using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.Helpers.Generals;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Entities;
using Karya.Core.Interfaces.Filters;
using Karya.Core.Interfaces.Repositories;
using Karya.Core.Interfaces.Services;
using Karya.Core.Interfaces.UnitOfWorks;
using Karya.Core.Results;

namespace Karya.Core.Services;

public abstract class BaseDetailService<TRepo, TEntity, TId,TParentFilter> : BaseService, IBaseDetailService<TEntity, TId, TParentFilter>
    where TRepo : class, IRepositoryAsync<TEntity, TId>
    where TEntity : class, IBaseEntity<TId>, new()
    where TParentFilter : IParentFilter, new()
{
    protected readonly IUnitOfWork _uow;

    private string _tableName = nameof(TEntity);


    protected BaseDetailService(IUnitOfWork uow)
    {
        _uow = uow;
    }


    public IQueryable<TEntity> Query(TParentFilter parentFilter)
    {
        var query = _uow.Repo<TRepo>(parentFilter).Query();
        return query;
    }

    public async virtual Task<BaseResult<LoadResult>> Select<TDto>(TParentFilter parentFilter, DataSourceLoadOptionsBase filterDataOptions) where TDto : class, ISelectDto, new()
    {

        var query = _uow.Repo<TRepo>(parentFilter).Query();


        if (filterDataOptions.Select == null || filterDataOptions.Select.Length == 0)
            filterDataOptions.Select = typeof(TDto).GetProperties().Select(p => (p.Name).FirstCharToLowerCase()).ToArray();

        var res = await DataSourceLoader.LoadAsync(query, filterDataOptions);

        return BaseResult<LoadResult>.Success("200", null, res);
    }

    public async Task<BaseResult<TDto>> ByKey<TDto>(TParentFilter parentFilter, TId key) where TDto : class, ISingleDto, new()
    {
        if (key == null)
            return BaseResult<TDto>.ErrorCoded("400", MessageCodes.Required, null, "Id");

        var entity = await _uow.Repo<TRepo>(parentFilter).GetByIdAsync(key);

        if (entity == null)
            return BaseResult<TDto>.ErrorCoded("404", MessageCodes.NotFound, null, _tableName, "Id", Convert.ToString(key));

        return BaseResult<TDto>.Success("200", null, EntityMapper.MapToDto<TEntity, TDto>(entity));

    }

   

    public virtual async Task<BaseResult> Insert<TDto>(TParentFilter parentFilter, TDto dto) where TDto : class, IInsertDto, new()
    {

        await _uow.Repo<TRepo>(parentFilter).AddAsync(EntityMapper.MapToEntity<TEntity, TDto>(dto));

        var result = await _uow.CompleteAsync();

        return new BaseResult<TDto>(result, data: dto);

    }

    public async Task<BaseResult> Update<TDto>(TParentFilter parentFilter, TId key, Dictionary<string, object> updateData) where TDto : class, IUpdateDto, new()
    {
        var dto = updateData.ToObject<TDto>();
        var entity = EntityMapper.MapToEntity<TEntity, TDto>(dto);
        entity.Id = key;
        var columns = UpdateDtoHelper.GetFieldsToUpdate<TDto, TEntity>(updateData);
        await _uow.Repo<TRepo>(parentFilter).UpdateAsync(entity, columns);
        var result = await _uow.CompleteAsync();

        return new BaseResult<TDto>(result, data: dto);
    }

    public async Task<BaseResult> Delete(TParentFilter parentFilter, TId key)
    {
        await _uow.Repo<TRepo>(parentFilter).DeleteAsync(key);
        var result = await _uow.CompleteAsync();

        return result;

    }

    public void Dispose()
    {
        this.Dispose();
    }
}


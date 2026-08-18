using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Karya.Core.Helpers.Generals;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Entities;
using Karya.Core.Interfaces.Repositories;
using Karya.Core.Interfaces.Services;
using Karya.Core.Interfaces.UnitOfWorks;
using Karya.Core.Results;

namespace Karya.Core.Services;

public abstract class BaseService : IBaseService
{
    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

public abstract class BaseService<TRepo, TEntity, TId> : BaseService, IBaseService<TEntity, TId>
    where TRepo : class, IRepositoryAsync<TEntity, TId>
    where TEntity : class, IBaseEntity<TId>, new()
{
    protected readonly IUnitOfWork _uow;

    private string _tableName = nameof(TEntity);


    protected BaseService(IUnitOfWork uow)
    {
        _uow = uow;
    }


    public IQueryable<TEntity> Query()
    {
        var query = _uow.Repo<TRepo>().Query();
        return query;
    }

    public async virtual Task<BaseResult<LoadResult>> Select<TDto>(DataSourceLoadOptionsBase filterDataOptions) where TDto : class, ISelectDto, new()
    {

        var query = _uow.Repo<TRepo>().Query();

        
        if(filterDataOptions.Select == null || filterDataOptions.Select.Length == 0)
            filterDataOptions.Select = typeof(TDto).GetProperties().Select(p => (p.Name).FirstCharToLowerCase()).ToArray();

        var res = await DataSourceLoader.LoadAsync(query, filterDataOptions);

        return BaseResult<LoadResult>.Success("200",null, res);
    }

    public async Task<BaseResult<TDto>> ByKey<TDto>(TId key) where TDto : class, IByKeyDto, new()
    {
        if (key == null) 
            return BaseResult<TDto>.ErrorCoded("400", MessageCodes.Required, null, "Id");

        var entity = await _uow.Repo<TRepo>().GetByIdAsync(key);

        if (entity == null) 
            return BaseResult<TDto>.ErrorCoded("404", MessageCodes.NotFound, null, _tableName, "Id", Convert.ToString(key));

        return BaseResult<TDto>.Success("200", null, EntityMapper.MapToDto<TEntity, TDto>(entity));

    }

    public virtual async Task<BaseResult> Insert<TDto>(TDto dto) where TDto : class, IInsertDto, new()
    {
        
        await _uow.Repo<TRepo>().AddAsync(EntityMapper.MapToEntity<TEntity, TDto>(dto));
        
        var result = await _uow.CompleteAsync();

        return new BaseResult<TDto>(result,data:dto);
        
    }

    public virtual async Task<BaseResult> Update<TDto>(TId key, Dictionary<string, object> updateData) where TDto : class, IUpdateDto, new()
    {
        var dto = updateData.ToObject<TDto>();
        var entity = EntityMapper.MapToEntity<TEntity, TDto>(dto);
        entity.Id = key;
        var columns = UpdateDtoHelper.GetFieldsToUpdate<TDto, TEntity>(updateData);
        await _uow.Repo<TRepo>().UpdateAsync(entity, columns);
        var result = await _uow.CompleteAsync();

        return new BaseResult<TDto>(result, data: dto);
    }

    public async Task<BaseResult> Delete(TId key)
    {
        await _uow.Repo<TRepo>().DeleteAsync(key);
        var result = await _uow.CompleteAsync();

        return result;

    }

    public override void Dispose()
    {
        _uow?.Dispose();         
        base.Dispose();
        GC.SuppressFinalize(this);
    }


}


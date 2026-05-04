//using Karya.Core.Helpers.Generals;
//using Karya.Core.Interfaces.DTOs;
//using Karya.Core.Interfaces.Entities;
//using Karya.Core.Interfaces.Repositories;
//using Karya.Core.Interfaces.Results;
//using Karya.Core.Interfaces.Services;
//using Karya.Core.Interfaces.UnitOfWorks;
//using Karya.Core.Results;
//using System.Linq.Expressions;

//namespace Karya.Core.Services;

//public abstract class BaseTanentService<TRepo, TEntity, TId> : IBaseService<TEntity, TId>
//    where TRepo : class, IRepositoryAsync<TEntity, TId>
//    where TEntity : class, IBaseEntity<TId>, new() 
//{
//    protected readonly ITanentUnitOfWork _uow;

//    protected BaseTanentService(ITanentUnitOfWork uow)
//    {
//        _uow = uow;
//    }

//    public IQueryable<TEntity> Query()
//    {
//        var query = _uow.Repo<TRepo>().Query();
//        return query;
//    }

//    public async Task<IBaseResult<IEnumerable<TDto>>> Select<TDto>(Expression<Func<TEntity, bool>> expression) where TDto: class, ISelectDto, new()
//    {
//        var entities = await _uow.Repo<TRepo>().GetAsync(expression);

//        var dtos = EntityMapper.MapToDto<TEntity,TDto>(entities);

//        return Result<IEnumerable<TDto>>.Success(dtos);
//    }

//    public async Task<IBaseResult<TDto>> ByKey<TDto>(TId key) where TDto : class, ISingleDto, new()
//    {
//        var entity = await _uow.Repo<TRepo>().GetByIdAsync(key);
//        if (entity == null)
//            return Result<TDto>.Error(null, "404", "Not Found");

//        return Result<TDto>.Success(EntityMapper.MapToDto<TEntity,TDto>(entity));
//    }

//    public virtual async Task<IBaseResult<TDto>> Insert<TDto>(TDto dto) where TDto : class, IInsertDto, new()
//    {
//        var entity = EntityMapper.MapToEntity<TEntity,TDto>(dto);
//        await _uow.Repo<TRepo>().AddAsync(entity);
//        var result = await _uow.CompleteAsync();

//        if (result.IsSuccess)
//            return Result<TDto>.Success(EntityMapper.MapToDto<TEntity, TDto>(entity), "201");
//        else
//            return Result<TDto>.Error(EntityMapper.MapToDto<TEntity, TDto>(entity), result.Code, result.Message);
//    }

//    public async Task<IBaseResult<TDto>> Update<TDto>(TId key, TDto dto) where TDto : class, IUpdateDto, new()
//    {
//        var entity = EntityMapper.MapToEntity<TEntity, TDto>(dto);
//        ((IBaseEntity<TId>)entity).Id = key;
//        var columns = DtoControl.GetActiveKeys(dto);
//        await _uow.Repo<TRepo>().UpdateAsync(entity,columns);
//        var result = await _uow.CompleteAsync();

//        if (result.IsSuccess)
//            return Result<TDto>.Success(dto,"200");
//        else
//            return Result<TDto>.Error(dto, result.Code, result.Message);
//    }

//    public async Task<IBaseResult> Delete(TId key)
//    {
//        await _uow.Repo<TRepo>().DeleteAsync(key);
//        var result = await _uow.CompleteAsync();

//        if (result.IsSuccess)
//            return Result.Success("200");
//        else
//            return Result.Error("400", result.Message);

//    }

//    public void Dispose()
//    {
//        this.Dispose();
//    }

   
//}

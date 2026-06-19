using Azure.Core;
using Karya.Core.Common.Data;
using Karya.Core.Helpers.Generals;
using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Entities;
using Karya.Core.Interfaces.Repositories;
using Karya.Core.Interfaces.Services;
using Karya.Core.Interfaces.UnitOfWorks;
using Karya.Core.Results;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Karya.Core.Services;


public static class StringExtensions
{
    public static string FirstCharToLowerCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        char firstChar = input[0];
        if (firstChar < 'A' || firstChar > 'Z')
            return input; // Not an English uppercase letter

        return string.Create(input.Length, input, (span, str) =>
        {
            str.CopyTo(span);
            span[0] = (char)(str[0] + 32); // Lowercases English character safely
        });
    } 
}

public abstract class BaseService : IBaseService
{
    public void Dispose()
    {
        throw new NotImplementedException();
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

    public async Task<BaseResult<LoadResult>> Select<TDto>(DataSourceLoadOptionsBase filterDataOptions) where TDto : class, ISelectDto, new()
    {

        var query = _uow.Repo<TRepo>().Query();

        filterDataOptions.Select = typeof(TDto).GetProperties().Select(p => (p.Name).FirstCharToLowerCase()).ToArray();

        var res = await DataSourceLoader.LoadAsync(query, filterDataOptions);

        return BaseResult<LoadResult>.Success("200",null, res);
    }

    public async Task<BaseResult> ByKey<TDto>(TId key) where TDto : class, ISingleDto, new()
    {
        if (key == null) 
            return BaseResult<TDto>.Error(code: "400", ServiceMessages.Required("Id"), null);

        var entity = await _uow.Repo<TRepo>().GetByIdAsync(key);

        if (entity == null) 
            return BaseResult<TDto>.Error(code: "404", ServiceMessages.NotFound(_tableName, "Id", Convert.ToString(key)), null);

        return BaseResult<TDto>.Success("200", null, EntityMapper.MapToDto<TEntity, TDto>(entity));

    }

    public virtual async Task<BaseResult> Insert<TDto>(TDto dto) where TDto : class, IInsertDto, new()
    {
        
        await _uow.Repo<TRepo>().AddAsync(EntityMapper.MapToEntity<TEntity, TDto>(dto));
        
        var result = await _uow.CompleteAsync();

        return new BaseResult<TDto>(result,data:dto);
        
    }

    public async Task<BaseResult> Update<TDto>(TId key, TDto dto) where TDto : class, IUpdateDto, new()
    {
        var entity = EntityMapper.MapToEntity<TEntity, TDto>(dto);
        entity.Id = key;
        var columns = DtoControl.GetActiveKeys(dto);
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

    public void Dispose()
    {
        this.Dispose();
    }


}


using Karya.Core.Interfaces.DTOs;
using Karya.Core.Interfaces.Entities;
using Karya.Core.Results;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karya.Core.Helpers.Generals;

public static class EntityMapper
{
    public static TEntity MapToEntity<TEntity, TDto>(TDto dto)
        where TEntity : class, IEntity, new()
        where TDto : class, IBaseDto, new()
    {
        var entity = dto.Adapt<TEntity>();

        return entity;
    }

    public static IEnumerable<TEntity> MapToEntity<TEntity, TDto>(IEnumerable<TDto> dtos)
        where TEntity : class, IBaseEntity, new()
        where TDto : class, IBaseDto, new()
    {
        var entities = dtos.Adapt<IEnumerable<TEntity>>();

        return entities;
    }


    public static TDto MapToDto<TEntity, TDto>(TEntity entity)
        where TEntity : class, new()
        where TDto : class, new()
    {
        var dto = entity.Adapt<TDto>();

        return dto;
    }



    public static IEnumerable<TDto> MapToDto<TEntity, TDto>(IEnumerable<TEntity> entities)
        where TEntity : class, new()
        where TDto : class, new()
    {
        var dtos = entities.Adapt<IEnumerable<TDto>>();

        return dtos;
    }
}

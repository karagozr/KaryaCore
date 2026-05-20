namespace Karya.Core.Interfaces.Entities.Tanent;


public interface IBusinessTanentEntity<TId, TTanentId> : IBaseTenantEntity<TId, TTanentId>, IBusinessEntity<TId>
{

}

public interface IBusinessTanentEntity<TId> : IBusinessTanentEntity<TId, string>
{

}


public interface IBusinessTanentDetailEntity<TId, TTanentId> : IBaseTenantEntity<TId, TTanentId>
{
    int RowNum { get; set; }
}

public interface IBusinessTanentDetailEntity<TId> : IBusinessTanentDetailEntity<TId, string>
{

}
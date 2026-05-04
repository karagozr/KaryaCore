namespace Karya.Core.Interfaces.Entities;

public interface IBusinessEntity<TId>:IBaseEntity<TId>, IVersionable, ISoftDelete
{
   
}

public interface IBusinessDetailEntity<TId> :IBaseEntity<TId>, IVersionable, ISoftDelete
{
    int RowNum { get; set; }
}

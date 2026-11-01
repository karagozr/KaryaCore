using Karya.Core.Identity.Domains.Entities;

namespace Karya.Core.Identity.Application.Services;

/// <summary>
/// Kütüphane içinde oluşturulan kullanıcı/rol nesnelerinin gerçek tipini belirler.
/// Genişletilmiş entity'ler (AppUser'dan türeyen) bu sayede seeder ve servislerde kullanılır.
/// </summary>
public interface IIdentityEntityFactory
{
    Type UserType { get; }

    Type RoleType { get; }

    AppUser CreateUser();

    AppRole CreateRole();
}

public class IdentityEntityFactory<TUser, TRole> : IIdentityEntityFactory
    where TUser : AppUser, new()
    where TRole : AppRole, new()
{
    public Type UserType => typeof(TUser);

    public Type RoleType => typeof(TRole);

    public virtual AppUser CreateUser() => new TUser();

    public virtual AppRole CreateRole() => new TRole();
}

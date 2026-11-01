using Karya.Core.Identity.Application.Services;
using Karya.Core.Interfaces.Identities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Base = Karya.Core.Identity.Controllers;

namespace Karya.Core.Identity.Controllers.Defaults;

// Kütüphanenin varsayılan (concrete) controller'ları. Sınıf adları abstract controller'larla aynıdır,
// böylece [controller] route'u değişmez. Uygulama ilgili abstract controller'dan türeyen kendi
// controller'ını yazarsa buradaki varsayılan controller eklenmez.

internal sealed class AppAuthController(IAppAuthService authService)
    : Base.AppAuthController(authService);

internal sealed class AppProfileController(IMediator mediator, IAppProfileService profileService)
    : Base.AppProfileController(mediator, profileService);

internal sealed class AppRoleController(IMediator mediator, AppRoleService service)
    : Base.AppRoleController(mediator, service);

internal sealed class AppRoleGroupController(IMediator mediator, AppRoleGroupService service)
    : Base.AppRoleGroupController(mediator, service);

internal sealed class AppRoleGroupRoleController(IMediator mediator, AppRoleGroupRoleService service)
    : Base.AppRoleGroupRoleController(mediator, service);

internal sealed class AppTenantController(IMediator mediator, AppTenantService service)
    : Base.AppTenantController(mediator, service);

internal sealed class AppUserController(IMediator mediator, IAppUserService appUserService)
    : Base.AppUserController(mediator, appUserService);

internal sealed class AppUserRoleController(DbContext context, ICurrentUser currentUser)
    : Base.AppUserRoleController(context, currentUser);

internal sealed class AppUserRoleGroupController(IMediator mediator, AppUserRoleGroupService service)
    : Base.AppUserRoleGroupController(mediator, service);


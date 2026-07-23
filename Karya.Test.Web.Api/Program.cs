using Karya.Core.App;
using Karya.Core.App.Interfaces.Services;
using Karya.Core.Indentity;
using Karya.Core.Indentity.Services;
using Karya.Core.Interfaces.Identities;
using Karya.Core.Interfaces.Localization;
using Karya.Core.Web.Identities;
using Karya.Core.Web.Infrastructure.Swagger;
using Karya.Test.Web.Api.Data;
using Karya.Test.Web.Api.Data.Service;
using Karya.Test.Web.Api.Localization;
using Karya.Test.Web.Api.Seeders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;



var builder = WebApplication.CreateBuilder(args);

var service = builder.Services;

builder.Services.AddControllers()
    .AddApplicationPart(Karya.Core.Indentity.AssemblyReference.Assembly);



builder.Services.AddCoreAppRegistiration();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Persist Security Info=True;Data Source=.;Initial Catalog=DEV_TEST;User ID=sa;Password=1234;Integrated Security=True;TrustServerCertificate=Yes"));


// Identity + OpenIddict kayıtları Karya.Core.Identity içinde toplandı.
builder.Services.AddCoreIdentityRegistiration<AppDbContext>();

builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IUserClaimsService, UserClaimsService>();
builder.Services.AddTransient<IClaimsTransformation, AppClaimsTransformer>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ERP API",
        Version = "v1"
    });

    // 🔴 Bearer tanımı
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Token gir: Bearer {token}"
    });

    // 🔴 Tüm endpointlere uygula
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
    options.OperationFilter<ParentRouteSwaggerFilter>();

});




//builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));




var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();


//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        options.UseSecurityTokenValidators = true;
//        options.TokenValidationParameters =
//            new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,
//                ValidIssuer = jwt.Issuer,
//                ValidAudience = jwt.Audience,
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
//            };
//    });

//builder.Services.AddAuthorization();
//builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Localization (DB-backed message catalog)
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IMessageLocalizer, DbMessageLocalizer>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteApp",
        policy =>
        {
            policy.AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowAnyOrigin();
        });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Önce roller ve kullanıcılar
        await IdentityDataSeeder.SeedUsersAsync(services);

        // Dil/çeviri kayıtları (varsayılan tr/en)
        await LocalizationSeeder.SeedAsync(services);
    }
    catch (Exception ex)
    {
        // Hata loglama
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowViteApp");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


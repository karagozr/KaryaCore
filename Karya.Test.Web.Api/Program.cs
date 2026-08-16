using Karya.Core.App;
using Karya.Core.Indentity;
using Karya.Core.Indentity.Services;
using Karya.Core.Interfaces.Localization;
using Karya.Test.Web.Api.Data;
using Karya.Test.Web.Api.Data.Service;
using Karya.Test.Web.Api.Localization;
using Karya.Test.Web.Api.Seeders;
using Microsoft.AspNetCore.Authentication;
using Scalar.AspNetCore;



var builder = WebApplication.CreateBuilder(args);

var service = builder.Services;

builder.Services.AddControllers()
    .AddApplicationPart(Karya.Core.Indentity.AssemblyReference.Assembly);



builder.Services.AddCoreAppRegistiration();

// Identity + OpenIddict kayıtları Karya.Core.Identity içinde toplandı.
builder.Services.AddCoreIdentityRegistiration<TestDbContext>(builder.Configuration, "AppConnection");
builder.Services.AddCoreSeeder<LocalizationSeeder>();

builder.Services.AddScoped<IUserClaimsService, UserClaimsService>();
builder.Services.AddTransient<IClaimsTransformation, AppClaimsTransformer>();
builder.Services.AddEndpointsApiExplorer();

// OpenAPI - İki ayrı dokuman: v1 (ERP) ve identity
var identityAssembly = Karya.Core.Indentity.AssemblyReference.Assembly;

static bool IsFromAssembly(Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription api, System.Reflection.Assembly assembly) =>
    api.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor cad &&
    cad.ControllerTypeInfo.Assembly == assembly;

//builder.Services.AddOpenApi("v1", options =>
//{
//    options.ShouldInclude = api => !IsFromAssembly(api, identityAssembly);
//});

//builder.Services.AddOpenApi("identity", options =>
//{
//    options.ShouldInclude = api => IsFromAssembly(api, identityAssembly);
//});

builder.Services.AddOpenApi();




//builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));


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
//builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();t

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

await app.Services.MigrateCoreDatabaseAsync<TestDbContext>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Karya API")
            .WithTheme(ScalarTheme.Saturn)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}
app.UseCors("AllowViteApp");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


using Karya.Core.Indentity.DTOs;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using System.Text.Json;

namespace Karya.Core.Indentity.Infrastructure;

public sealed class ExtractJsonTokenRequestHandler : IOpenIddictServerHandler<OpenIddictServerEvents.ExtractTokenRequestContext>
{
    public async ValueTask HandleAsync(OpenIddictServerEvents.ExtractTokenRequestContext context)
    {
        var request = context.Transaction.GetHttpRequest();

        if (request is null || !HttpMethods.IsPost(request.Method))
            return;

        if (request.ContentType?.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) == true)
        {
            var dto = await JsonSerializer.DeserializeAsync<AppLoginDto>(
                request.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                context.CancellationToken);

            if (dto is null)
            {
                context.Reject(error: OpenIddictConstants.Errors.InvalidRequest, description: "Invalid login request.");

                return;
            }

            context.Transaction.Request = new OpenIddictRequest
            {
                GrantType = OpenIddictConstants.GrantTypes.Password,
                Username = dto.UserName,
                Password = dto.Password
            };

            context.Transaction.Request["tenantId"] = dto.TenantId;

            return;
        }

        if (request.ContentType?.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) == true)
        {
            context.Transaction.Request = new OpenIddictRequest(
                await request.ReadFormAsync(context.CancellationToken));

            return;
        }

        context.Reject(
            error: OpenIddictConstants.Errors.InvalidRequest,
            description: "Content-Type must be application/json or application/x-www-form-urlencoded.");
    }
}
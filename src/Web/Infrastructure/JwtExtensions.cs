using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Web.Infrastructure;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Api:Authority"];
                options.Audience = configuration["Api:Audience"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudiences = configuration
                        .GetSection("Api:ValidAudiences")
                        .Get<string[]>(),
                    ValidIssuers = configuration
                        .GetSection("Api:ValidIssuers")
                        .Get<string[]>()
                };

                options.MapInboundClaims = false;
            });

        return services;
    }
}

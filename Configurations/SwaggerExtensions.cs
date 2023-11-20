using System.Reflection;
using GenericWebAPI.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;


namespace GenericWebAPI.Configurations;

public static class SwaggerExtensions
{
    public static IServiceCollection ConfigureSwaggerWithAuth(this IServiceCollection services, IConfiguration configuration,
        bool auth = true)
    {
        if (auth)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));
        }

        services.AddSwaggerGen(c =>
        {
            var scopesArray = configuration.GetSection("AzureAd:Swagger")["Scopes-optional"].Split(",")
                .Select(s => s.Trim())
                .ToArray();
            
            c.OperationFilter<SummaryOperationFilter>();

            c.EnableAnnotations();
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = configuration.GetSection("AzureAd:Swagger:Api")["Name"],
                    Version = configuration.GetSection("AzureAd:Swagger:Api")["Version"]
                });
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl =
                            new Uri(
                                $"https://login.microsoftonline.com/{configuration.GetSection("AzureAd")["TenantId"]}/oauth2/v2.0/authorize"),
                        TokenUrl = new Uri(
                            $"https://login.microsoftonline.com/{configuration.GetSection("AzureAd")["TenantId"]}/oauth2/v2.0/token"),
                        Scopes = scopesArray.ToDictionary(s => s, s => s)
                    }
                }
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                    },
                    scopesArray
                }
            });
        });
        
        return services;
    }

    public static IApplicationBuilder ConfigureSwaggerUIWithAuth(this IApplicationBuilder app, IConfiguration configuration, bool auth = true)
    {
        if (auth)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var scopesArray = configuration.GetSection("AzureAd:Swagger")["Scopes-selected"].Split(",")
                .Select(s => s.Trim())
                .ToArray();

            options.EnableDeepLinking();
            options.DisplayRequestDuration();
            options.OAuthClientId(configuration.GetSection("AzureAd")["ClientId"]);
            options.OAuthScopes(scopesArray);
            options.OAuthUsePkce();
            options.OAuthScopeSeparator(" ");
            options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
        });

        return app;
    }
}
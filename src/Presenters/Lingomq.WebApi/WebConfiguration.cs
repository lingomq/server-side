using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LingoMQ.Presenters.WebApi;

public static class WebConfiguration
{
    public static IMvcBuilder ConfigureControllers(this IMvcBuilder builder)
    {
        builder
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
                    .Json
                    .ReferenceLoopHandling
                    .Ignore
            )
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context
                        .ModelState.Values.SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage);

                    return new BadRequestObjectResult(
                        new { Message = "Ошибки валидации", Errors = errors }
                    );
                };
            });

        return builder;
    }

    public static IServiceCollection AddJwtAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtConfiguration:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtConfiguration:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtConfiguration:Key"]!)
                    ),
                };
            });

        return services;
    }

    public static IServiceCollection AddSwagger(
        this IServiceCollection services,
        string serviceName = ""
    )
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                }
            );

            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Jwt",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        Array.Empty<string>()
                    },
                }
            );
        });

        return services;
    }
}

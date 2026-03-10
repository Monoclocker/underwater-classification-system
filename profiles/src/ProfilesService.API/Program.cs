using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProfilesService.API.Extensions;
using ProfilesService.API.Options;
using ProfilesService.Infrastructure.Cache;
using ProfilesService.Infrastructure.Database.Extensions;
using ProfilesService.Infrastructure.Database.Options;

var builder = WebApplication.CreateBuilder(args);

DatabaseConfigurationOptions databaseConfiguration = builder
                                           .Configuration
                                           .GetSection("Database")
                                           .Get<DatabaseConfigurationOptions>()
                                       ?? throw new ArgumentException("Database configuration was not provided");

JwtOptions jwtOptions = builder
                            .Configuration
                            .GetSection("Jwt")
                            .Get<JwtOptions>()
                        ?? throw new ArgumentException("JWT configuration was not provided");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience
        };
    });
    
builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

builder.Services.AddDatabase(databaseConfiguration);
builder.Services.AddRedisDistributedCache(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.AddEndpoints();

app.Run();
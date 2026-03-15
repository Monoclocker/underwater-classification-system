using DevicesService.Domain;
using DevicesService.Infrastructure;
using DevicesService.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

DatabaseConnectionOptions databaseConnectionOptions = builder
    .Configuration
    .GetSection("Database")
    .Get<DatabaseConnectionOptions>() ?? throw new ArgumentException("Database configuration is missing"); 

builder.Services.AddDatabase(databaseConnectionOptions);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = "DeviceService";
});

builder.Services.AddServices();

builder.Services.AddOpenApi();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();

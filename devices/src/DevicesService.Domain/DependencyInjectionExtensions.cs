using DevicesService.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DevicesService.Domain;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddServices()
        {
            services.AddScoped<IDevicesService, Services.DevicesService>();
        }
    }
}
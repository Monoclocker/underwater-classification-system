using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ProfilesService.Handlers.Commands;
using ProfilesService.Handlers.Queries;

namespace ProfilesService.Handlers;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddHandlers()
        {
            services.AddCommandHandlers();
            services.AddQueryHandlers();
        }

        private void AddCommandHandlers()
        {
            var handlerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t is { IsInterface: false, IsAbstract: false }
                            && t.GetInterfaces().Any(i => 
                                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)));

            foreach (var handlerType in handlerTypes)
            {
                var handlerInterface = handlerType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

                services.AddScoped(handlerInterface, handlerType);
            }
        }

        private void AddQueryHandlers()
        {
            var handlerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t is { IsInterface: false, IsAbstract: false }
                            && t.GetInterfaces().Any(i => 
                                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var handlerType in handlerTypes)
            {
                var handlerInterface = handlerType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

                services.AddScoped(handlerInterface, handlerType);
            }
        }
    }
}
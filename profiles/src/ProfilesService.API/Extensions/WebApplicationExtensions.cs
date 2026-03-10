namespace ProfilesService.API.Extensions;

public static class WebApplicationExtensions
{
    extension(WebApplication app)
    {
        public void AddEndpoints()
        {
            foreach (var endpoint in app
                         .Services
                         .GetRequiredService<IEnumerable<IEndpoint>>())
                endpoint.Map(app);
        }
    }
}
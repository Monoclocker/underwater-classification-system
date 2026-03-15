namespace DevicesService.Infrastructure.Database;

public sealed class DatabaseConnectionOptions
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string Database { get; init; }
}
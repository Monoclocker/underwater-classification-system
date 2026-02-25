using FluentResults;

namespace ProfilesService.Handlers.Commands;

public interface ICommandHandler<in TCommand, TResponse> where TCommand: BaseCommand
{
    Task<Result<TResponse>> HandleAsync(TCommand command);
}

public interface ICommandHandler<in TCommand> where TCommand: BaseCommand
{
    Task<Result> HandleAsync(TCommand command);
}
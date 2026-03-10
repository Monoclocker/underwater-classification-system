using FluentResults;

namespace ProfilesService.Handlers.Commands;

public interface ICommandHandler<in TCommand, TResponse> where TCommand: BaseCommand
{
    Task<TResponse> HandleAsync(TCommand command);
}
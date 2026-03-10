namespace ProfilesService.Handlers.Queries;

public interface IQueryHandler<in TQuery, TResult> where TQuery : BaseQuery
{
    Task<TResult> HandleAsync(TQuery query);
}
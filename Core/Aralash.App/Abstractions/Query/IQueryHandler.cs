namespace Aralash.App.Abstractions.Query;

public interface IQueryHandler<in TResponse, TRequest> : IRequestHandler<TResponse, TRequest>
    where TResponse : IRequest<TRequest>;

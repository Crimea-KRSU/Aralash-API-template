namespace Aralash.App.Abstractions.Query;

public interface IQuery<out TResponse> : IRequest<TResponse>;
namespace Aralash.App.Abstractions.Command;

public interface ICommand<out TResponse> : IRequest<TResponse>;
public interface ICommand : IRequest;
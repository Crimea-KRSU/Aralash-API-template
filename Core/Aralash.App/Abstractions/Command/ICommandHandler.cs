namespace Aralash.App.Abstractions.Command;

public interface ICommandHandler<in TResponse, TRequest> : IRequestHandler<TResponse, TRequest>
    where TResponse : ICommand<TRequest>;

public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest> where TRequest : ICommand;

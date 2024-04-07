namespace Aralash.App.Behaviors;

public class ClaimsValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly ISecuredOperationsManager _manager;
    private readonly ICurrentUser _currentUser;
    private readonly IAralashDbContext _uow;

    public ClaimsValidationBehavior(ICurrentUser currentUser, IAralashDbContext uow, ISecuredOperationsManager manager)
    {
        _currentUser = currentUser;
        _uow = uow;
        _manager = manager;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // не авторизован, а значит вызывающий медиатор эндпоинт был анонимным, пропускаем
        if (_currentUser.UserId == null)
            return await next();

        var user = await _uow.Users.FirstOrDefaultAsync(x => x.Id == _currentUser.UserId, cancellationToken);

        // сомнительно, что такое случится, но лишним не будет
        if (user == null)
            throw new AuthenticationException("Информация о пользователе не найдена");

        if (!user.IsActive)
            throw new UnauthorizedAccessException(
                "Ваша учетная запись не активирована или отключена. Обратитесь к администратору");

        if (!await _manager.ValidateExecute<TRequest>(_currentUser.UserId, cancellationToken))
            throw new UnauthorizedAccessException("У вас нет прав на выполнение данной команды");

        return await next();
    }
}
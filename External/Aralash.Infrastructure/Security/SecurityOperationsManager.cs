using System.Collections.Concurrent;

namespace Aralash.Infrastructure.Security;

public class SecuredOperationsManager : ISecuredOperationsManager
{
    private readonly IAralashDbContext _dbContext;
    private static ConcurrentDictionary<string, SecuredOperation>? _securedOperationsCache = new();

    public SecuredOperationsManager(IAralashDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void ScanAndUpdateSecuredOperations(Assembly assembly)
    {
        var types = ScanForSecuredOperations(assembly).ToList();
        var existedSp = _dbContext.SecuredOperations.AsEnumerable()
            .ToDictionary(x => x.OperationName, x => x);
        _securedOperationsCache = new(existedSp);

        if (!existedSp.ContainsKey(SuperAdminCommand.Name))
        {
            _dbContext.SecuredOperations.AddAsync(new SecuredOperation()
            {
                CreateDate = DateTime.UtcNow,
                Description = SuperAdminCommand.Description,
                Id = Guid.NewGuid().ToString(),
                OperationName = SuperAdminCommand.Name
            });
        }
        
        var newSp = new List<SecuredOperation>(types.Count / 2);
        
        foreach (var type in types)
        {
            var attribute = type.GetCustomAttribute<SecuredOperationAttribute>()!;
            if (existedSp.TryGetValue(type.Name, out var sp))
            {
                if(!string.Equals(sp.OperationName, attribute.Description, StringComparison.OrdinalIgnoreCase))
                    sp.Description = attribute.Description;
            }
            else
            {
                newSp.Add(new SecuredOperation()
                {
                    Description = attribute.Description,
                    Id = Guid.NewGuid().ToString(),
                    OperationName = type.Name,
                    CreateDate = DateTime.UtcNow
                });
            }
        }
        
        if(newSp.Count != 0)
            _dbContext.SecuredOperations.AddRange(newSp); 
        _dbContext.SaveChanges();
    }

    public async Task<bool> ValidateExecute<TRequest>(string userId, CancellationToken ct)
    {
        if (_securedOperationsCache == null)
        {
            throw new InvalidOperationException(
                $"Сборка не была просканирована на наличие защищаемых операций. " +
                $"Вызовите метод {nameof(ISecuredOperationsManager.ScanAndUpdateSecuredOperations)} в исходной точке приложения для сканирования");
        }
        
        var soName = typeof(TRequest).Name;
        // анонимная ли команда?
        if (!_securedOperationsCache.ContainsKey(soName))
            return true;
        
        var userAllowedOperations = _dbContext.UserRoles
            .Where(x => x.UserId == userId)
            .Include(x => x.Role.Operations)
                .ThenInclude(x => x.Operation)
            .SelectMany(x => x.Role.Operations.Select(z => z.Operation.OperationName));

        return await userAllowedOperations.AnyAsync(x => x == soName || x == SuperAdminCommand.Name, ct);
    }
    
    private IEnumerable<Type> ScanForSecuredOperations(Assembly assembly)
    {
        return assembly.GetTypes().Where(type =>
            Attribute.IsDefined(type, typeof(SecuredOperationAttribute)) && (typeof(IRequest).IsAssignableFrom(type) ||
                                                                             type.GetInterfaces().Any(i =>
                                                                                 i.IsGenericType &&
                                                                                 i.GetGenericTypeDefinition() ==
                                                                                 typeof(IRequest<>))));
    }
}

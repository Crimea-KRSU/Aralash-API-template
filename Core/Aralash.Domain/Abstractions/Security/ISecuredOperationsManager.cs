using System.Reflection;

namespace Aralash.Domain.Abstractions.Security;

public interface ISecuredOperationsManager
{
    void ScanAndUpdateSecuredOperations(Assembly assembly);

    Task<bool> ValidateExecute<TRequest>(string userId, CancellationToken ct);
}
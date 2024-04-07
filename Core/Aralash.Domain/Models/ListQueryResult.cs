namespace Aralash.Domain.Models;

public record ListQueryResult<T>(List<T> Collection, int TotalCount)
{
    public static ListQueryResult<T> Empty() => new(new List<T>(), 0);
}
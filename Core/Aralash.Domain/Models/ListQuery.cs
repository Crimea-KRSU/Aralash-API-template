namespace Aralash.Domain.Models;

public record ListQuery<T> : IRequest<ListQueryResult<T>>
{
    public int Page { get; set; }
    public int PerPage { get; set; }
}
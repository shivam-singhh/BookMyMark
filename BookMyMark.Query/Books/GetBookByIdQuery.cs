using BookMyMark.AppService.Interfaces;
using BookMyMark.Shared.Models;

namespace BookMyMark.Query.Books;

public sealed record GetBookByIdQuery(int Id, CancellationToken CancellationToken = default);

public sealed class GetBookByIdQueryHandler(IBookAppService service)
{
    public Task<Book?> HandleAsync(GetBookByIdQuery query) =>
        service.GetByIdAsync(query.Id, query.CancellationToken);
}

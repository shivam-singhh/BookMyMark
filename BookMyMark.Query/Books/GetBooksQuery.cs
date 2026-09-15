using BookMyMark.AppService.Interfaces;
using BookMyMark.Shared.Models;

namespace BookMyMark.Query.Books;

public sealed record GetBooksQuery(CancellationToken CancellationToken = default);

public sealed class GetBooksQueryHandler(IBookAppService service)
{
    public Task<IReadOnlyList<Book>> HandleAsync(GetBooksQuery query) =>
        service.GetAllAsync(query.CancellationToken);
}

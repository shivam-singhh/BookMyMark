using BookMyMark.AppService.Interfaces;
using BookMyMark.Shared.Models;

namespace BookMyMark.Query.ReadingList;

public sealed record GetReadingListItemQuery(int Id, CancellationToken CancellationToken = default);

public sealed class GetReadingListItemQueryHandler(IReadingListAppService service)
{
    public Task<ReadingListItem?> HandleAsync(GetReadingListItemQuery query) =>
        service.GetByIdAsync(query.Id, query.CancellationToken);
}

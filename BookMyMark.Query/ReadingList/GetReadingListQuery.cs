using BookMyMark.AppService.Interfaces;
using BookMyMark.Shared.Models;

namespace BookMyMark.Query.ReadingList;

public sealed record GetReadingListQuery(ReadingStatus? Status, CancellationToken CancellationToken = default);

public sealed class GetReadingListQueryHandler(IReadingListAppService service)
{
    public Task<IReadOnlyList<ReadingListItem>> HandleAsync(GetReadingListQuery query) =>
        service.GetAllAsync(query.Status, query.CancellationToken);
}

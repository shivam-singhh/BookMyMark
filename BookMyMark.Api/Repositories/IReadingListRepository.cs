using BookMyMark.Api.Models;

namespace BookMyMark.Api.Repositories;

public interface IReadingListRepository
{
    Task<IReadOnlyList<ReadingListItem>> GetAllAsync(
        ReadingStatus? status = null,
        CancellationToken cancellationToken = default);

    Task<ReadingListItem?> GetByIdAsync(
        int id,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<ReadingListItem> AddAsync(
        ReadingListItem item,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
using BookMyMark.Shared.Models;

namespace BookMyMark.AppService.Interfaces;

public interface IReadingListAppService
{
    Task<IReadOnlyList<ReadingListItem>> GetAllAsync(ReadingStatus? status = null, CancellationToken cancellationToken = default);
    Task<ReadingListItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ReadingListItem> AddAsync(int bookId, CancellationToken cancellationToken = default);
    Task<ReadingListItem?> UpdateStatusAsync(int id, ReadingStatus status, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

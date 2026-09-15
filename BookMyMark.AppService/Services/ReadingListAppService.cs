using BookMyMark.AppService.Interfaces;
using BookMyMark.Infrastructure.Repositories;
using BookMyMark.Infrastructure.Services;
using BookMyMark.Shared.Models;

namespace BookMyMark.AppService.Services;

public sealed class ReadingListAppService(
    IReadingListRepository repository,
    IBookCatalogService catalogService) : IReadingListAppService
{
    public Task<IReadOnlyList<ReadingListItem>> GetAllAsync(
        ReadingStatus? status = null,
        CancellationToken cancellationToken = default) =>
        repository.GetAllAsync(status, cancellationToken);

    public Task<ReadingListItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        repository.GetByIdAsync(id, cancellationToken: cancellationToken);

    public async Task<ReadingListItem> AddAsync(int bookId, CancellationToken cancellationToken = default)
    {
        if (await catalogService.GetByIdAsync(bookId, cancellationToken) is null)
        {
            throw new ArgumentException($"Book with ID {bookId} does not exist.", nameof(bookId));
        }

        return await repository.AddAsync(new ReadingListItem { BookId = bookId }, cancellationToken);
    }

    public async Task<ReadingListItem?> UpdateStatusAsync(
        int id,
        ReadingStatus status,
        CancellationToken cancellationToken = default)
    {
        var item = await repository.GetByIdAsync(id, true, cancellationToken);
        if (item is null) return null;

        item.Status = status;
        item.FinishedAt = status == ReadingStatus.Finished ? DateTime.UtcNow : null;
        await repository.SaveChangesAsync(cancellationToken);
        return item;
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) =>
        repository.DeleteAsync(id, cancellationToken);
}

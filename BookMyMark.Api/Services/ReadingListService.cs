using BookMyMark.Api.Models;
using BookMyMark.Api.Repositories;

namespace BookMyMark.Api.Services;

public sealed class ReadingListService(
    IReadingListRepository readingListRepository,
    IBookService bookService) : IReadingListService
{
    public async Task<IReadOnlyList<ReadingListItem>> GetAllAsync(
        ReadingStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        return await readingListRepository.GetAllAsync(status, cancellationToken);
    }

    public Task<ReadingListItem?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return readingListRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
    }

    public async Task<ReadingListItem> AddAsync(
        int bookId,
        CancellationToken cancellationToken = default)
    {
        if (await bookService.GetByIdAsync(bookId, cancellationToken) is null)
        {
            throw new ArgumentException($"Book with ID {bookId} does not exist.", nameof(bookId));
        }

        var item = new ReadingListItem { BookId = bookId };
        return await readingListRepository.AddAsync(item, cancellationToken);
    }

    public async Task<ReadingListItem?> UpdateStatusAsync(
        int id,
        ReadingStatus status,
        CancellationToken cancellationToken = default)
    {
        var item = await readingListRepository.GetByIdAsync(
            id,
            trackChanges: true,
            cancellationToken);

        if (item is null)
        {
            return null;
        }

        item.Status = status;
        item.FinishedAt = status == ReadingStatus.Finished ? DateTime.UtcNow : null;
        await readingListRepository.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await readingListRepository.DeleteAsync(id, cancellationToken);
    }
}

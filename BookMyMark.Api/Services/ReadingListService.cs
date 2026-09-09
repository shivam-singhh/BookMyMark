using BookMyMark.Api.Data;
using BookMyMark.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyMark.Api.Services;

public sealed class ReadingListService(
    BookMyMarkDbContext dbContext,
    IBookService bookService) : IReadingListService
{
    public async Task<IReadOnlyList<ReadingListItem>> GetAllAsync(
        ReadingStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.ReadingListItems.AsNoTracking();

        if (status.HasValue)
        {
            query = query.Where(item => item.Status == status.Value);
        }

        return await query
            .OrderByDescending(item => item.AddedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<ReadingListItem?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.ReadingListItems
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
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
        dbContext.ReadingListItems.Add(item);
        await dbContext.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task<ReadingListItem?> UpdateStatusAsync(
        int id,
        ReadingStatus status,
        CancellationToken cancellationToken = default)
    {
        var item = await dbContext.ReadingListItems
            .FirstOrDefaultAsync(readingItem => readingItem.Id == id, cancellationToken);

        if (item is null)
        {
            return null;
        }

        item.Status = status;
        item.FinishedAt = status == ReadingStatus.Finished ? DateTime.UtcNow : null;
        await dbContext.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var item = await dbContext.ReadingListItems
            .FirstOrDefaultAsync(readingItem => readingItem.Id == id, cancellationToken);

        if (item is null)
        {
            return false;
        }

        dbContext.ReadingListItems.Remove(item);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}

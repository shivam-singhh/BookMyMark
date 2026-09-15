using BookMyMark.Api.Data;
using BookMyMark.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyMark.Api.Repositories;

public sealed class ReadingListRepository(BookMyMarkDbContext dbContext)
    : IReadingListRepository
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
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        var query = trackChanges
            ? dbContext.ReadingListItems
            : dbContext.ReadingListItems.AsNoTracking();

        return query.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
    }

    public async Task<ReadingListItem> AddAsync(
        ReadingListItem item,
        CancellationToken cancellationToken = default)
    {
        dbContext.ReadingListItems.Add(item);
        await dbContext.SaveChangesAsync(cancellationToken);
        return item;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var item = await GetByIdAsync(id, trackChanges: true, cancellationToken);

        if (item is null)
        {
            return false;
        }

        dbContext.ReadingListItems.Remove(item);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
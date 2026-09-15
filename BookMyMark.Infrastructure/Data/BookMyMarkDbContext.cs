using BookMyMark.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyMark.Infrastructure.Data;

public class BookMyMarkDbContext(DbContextOptions<BookMyMarkDbContext> options)
    : DbContext(options)
{
    public DbSet<ReadingListItem> ReadingListItems => Set<ReadingListItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReadingListItem>()
            .Property(item => item.Status)
            .HasConversion<string>();
    }
}

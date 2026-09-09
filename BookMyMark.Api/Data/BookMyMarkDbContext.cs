using BookMyMark.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyMark.Api.Data;

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

using BookMyMark.Shared.Models;

namespace BookMyMark.Infrastructure.Services;

public interface IBookCatalogService
{
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

using BookMyMark.Api.Models;

namespace BookMyMark.Api.Services;

public interface IBookService
{
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

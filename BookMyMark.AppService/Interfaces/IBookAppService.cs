using BookMyMark.Shared.Models;

namespace BookMyMark.AppService.Interfaces;

public interface IBookAppService
{
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

using BookMyMark.AppService.Interfaces;
using BookMyMark.Infrastructure.Services;
using BookMyMark.Shared.Models;

namespace BookMyMark.AppService.Services;

public sealed class BookAppService(IBookCatalogService catalogService) : IBookAppService
{
    public Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default) =>
        catalogService.GetAllAsync(cancellationToken);

    public Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        catalogService.GetByIdAsync(id, cancellationToken);
}

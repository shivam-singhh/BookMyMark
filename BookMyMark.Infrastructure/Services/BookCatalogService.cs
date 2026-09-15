using System.Text.Json;
using BookMyMark.Shared.Models;
using Microsoft.AspNetCore.Hosting;

namespace BookMyMark.Infrastructure.Services;

public sealed class BookCatalogService(IWebHostEnvironment environment) : IBookCatalogService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await LoadBooksAsync(cancellationToken);

    public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        (await LoadBooksAsync(cancellationToken)).FirstOrDefault(book => book.Id == id);

    private async Task<List<Book>> LoadBooksAsync(CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(environment.ContentRootPath, "Data", "books.json");
        await using var stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<List<Book>>(stream, JsonOptions, cancellationToken) ?? [];
    }
}

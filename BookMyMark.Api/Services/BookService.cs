using System.Text.Json;
using BookMyMark.Api.Models;

namespace BookMyMark.Api.Services;

public sealed class BookService(IWebHostEnvironment environment) : IBookService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await LoadBooksAsync(cancellationToken);
    }

    public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var books = await LoadBooksAsync(cancellationToken);
        return books.FirstOrDefault(book => book.Id == id);
    }

    private async Task<List<Book>> LoadBooksAsync(CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(environment.ContentRootPath, "Data", "books.json");
        await using var stream = File.OpenRead(filePath);

        var books = await JsonSerializer.DeserializeAsync<List<Book>>(
            stream,
            JsonOptions,
            cancellationToken);

        return books ?? [];
    }
}

using BookMyMark.Query.Books;
using BookMyMark.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMyMark.Api.Features.Books;

[ApiController]
[Route("api/books")]
public class BooksController(
    GetBooksQueryHandler getBooks,
    GetBookByIdQueryHandler getBookById) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Book>>> GetAll(
        CancellationToken cancellationToken)
    {
        var books = await getBooks.HandleAsync(new GetBooksQuery(cancellationToken));
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var book = await getBookById.HandleAsync(new GetBookByIdQuery(id, cancellationToken));
        return book is null ? NotFound() : Ok(book);
    }
}

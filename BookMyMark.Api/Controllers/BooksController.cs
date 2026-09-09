using BookMyMark.Api.Models;
using BookMyMark.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMyMark.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Book>>> GetAll(
        CancellationToken cancellationToken)
    {
        var books = await bookService.GetAllAsync(cancellationToken);
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var book = await bookService.GetByIdAsync(id, cancellationToken);

        return book is null ? NotFound() : Ok(book);
    }
}

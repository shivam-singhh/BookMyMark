using BookMyMark.Api.Models;
using BookMyMark.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMyMark.Api.Controllers;

[ApiController]
[Route("api/reading-list")]
public class ReadingListController(IReadingListService readingListService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ReadingListItem>>> GetAll(
        [FromQuery] ReadingStatus? status,
        CancellationToken cancellationToken)
    {
        var items = await readingListService.GetAllAsync(status, cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadingListItem>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var item = await readingListService.GetByIdAsync(id, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ReadingListItem>> Add(
        [FromBody] ReadingListItem request,
        CancellationToken cancellationToken)
    {
        try
        {
            var item = await readingListService.AddAsync(request.BookId, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<ReadingListItem>> UpdateStatus(
        int id,
        [FromBody] UpdateReadingStatusRequest request,
        CancellationToken cancellationToken)
    {
        var item = await readingListService.UpdateStatusAsync(id, request.Status, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await readingListService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

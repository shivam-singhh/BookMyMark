using BookMyMark.Command.ReadingList;
using BookMyMark.DTO.Requests;
using BookMyMark.Query.ReadingList;
using BookMyMark.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookMyMark.Api.Features.ReadingList;

[ApiController]
[Route("api/reading-list")]
public class ReadingListController(
    GetReadingListQueryHandler getReadingList,
    GetReadingListItemQueryHandler getReadingListItem,
    AddReadingListItemCommandHandler addReadingListItem,
    UpdateReadingStatusCommandHandler updateReadingStatus,
    DeleteReadingListItemCommandHandler deleteReadingListItem) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ReadingListItem>>> GetAll(
        [FromQuery] ReadingStatus? status,
        CancellationToken cancellationToken)
    {
        var items = await getReadingList.HandleAsync(
            new GetReadingListQuery(status, cancellationToken));
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadingListItem>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var item = await getReadingListItem.HandleAsync(
            new GetReadingListItemQuery(id, cancellationToken));
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ReadingListItem>> Add(
        [FromBody] AddReadingListItemRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var item = await addReadingListItem.HandleAsync(
                new AddReadingListItemCommand(request.BookId, cancellationToken));
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
        var item = await updateReadingStatus.HandleAsync(
            new UpdateReadingStatusCommand(id, request.Status, cancellationToken));
        return item is null ? NotFound() : Ok(item);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await deleteReadingListItem.HandleAsync(
            new DeleteReadingListItemCommand(id, cancellationToken));
        return deleted ? NoContent() : NotFound();
    }
}

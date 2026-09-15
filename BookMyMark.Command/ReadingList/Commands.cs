using BookMyMark.AppService.Interfaces;
using BookMyMark.Shared.Models;

namespace BookMyMark.Command.ReadingList;

public sealed record AddReadingListItemCommand(int BookId, CancellationToken CancellationToken = default);
public sealed record UpdateReadingStatusCommand(int Id, ReadingStatus Status, CancellationToken CancellationToken = default);
public sealed record DeleteReadingListItemCommand(int Id, CancellationToken CancellationToken = default);

public sealed class AddReadingListItemCommandHandler(IReadingListAppService service)
{
    public Task<ReadingListItem> HandleAsync(AddReadingListItemCommand command) =>
        service.AddAsync(command.BookId, command.CancellationToken);
}

public sealed class UpdateReadingStatusCommandHandler(IReadingListAppService service)
{
    public Task<ReadingListItem?> HandleAsync(UpdateReadingStatusCommand command) =>
        service.UpdateStatusAsync(command.Id, command.Status, command.CancellationToken);
}

public sealed class DeleteReadingListItemCommandHandler(IReadingListAppService service)
{
    public Task<bool> HandleAsync(DeleteReadingListItemCommand command) =>
        service.DeleteAsync(command.Id, command.CancellationToken);
}

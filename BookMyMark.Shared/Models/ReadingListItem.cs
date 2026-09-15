namespace BookMyMark.Shared.Models;

public class ReadingListItem
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public ReadingStatus Status { get; set; } = ReadingStatus.Reading;
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; set; }
}

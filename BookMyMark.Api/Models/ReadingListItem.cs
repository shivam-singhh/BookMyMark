using System.ComponentModel.DataAnnotations;

namespace BookMyMark.Api.Models;

public class ReadingListItem
{
    public int Id { get; set; }

    [Range(1, int.MaxValue)]
    public int BookId { get; set; }

    public ReadingStatus Status { get; set; } = ReadingStatus.Reading;

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public DateTime? FinishedAt { get; set; }
}

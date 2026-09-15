using System.ComponentModel.DataAnnotations;

namespace BookMyMark.DTO.Requests;

public sealed class AddReadingListItemRequest
{
    [Range(1, int.MaxValue)]
    public int BookId { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace BookMyMark.Api.Models;

public class UpdateReadingStatusRequest
{
    [Required]
    public ReadingStatus Status { get; set; }
}

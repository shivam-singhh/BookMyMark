using System.ComponentModel.DataAnnotations;
using BookMyMark.Shared.Models;

namespace BookMyMark.DTO.Requests;

public sealed class UpdateReadingStatusRequest
{
    [Required]
    public ReadingStatus Status { get; set; }
}

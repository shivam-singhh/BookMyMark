using System.ComponentModel.DataAnnotations;

namespace BookMyMark.Api.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public required string Title { get; set; }

    [Required]
    [StringLength(150)]
    public required string Author { get; set; }

    [StringLength(20)]
    public string? Isbn { get; set; }

    [Range(0, 9999)]
    public int PublishedYear { get; set; }

    [StringLength(100)]
    public string? Genre { get; set; }

    [Url]
    public string? CoverImageUrl { get; set; }
}

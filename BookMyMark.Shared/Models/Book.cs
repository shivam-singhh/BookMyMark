namespace BookMyMark.Shared.Models;

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public string? Isbn { get; set; }
    public int PublishedYear { get; set; }
    public string? Genre { get; set; }
    public string? CoverImageUrl { get; set; }
}

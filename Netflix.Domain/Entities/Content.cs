namespace Netflix.Domain.Entities;

public abstract class Content
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int ReleaseYear { get; init; }
    public string AgeRating { get; init; } = string.Empty;
    public string Language { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public double AverageRating { get; init; }
    public int GenreId { get; set; }

    public Genre Genre { get; init; } = null!;
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
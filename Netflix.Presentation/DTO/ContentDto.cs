namespace Netflix.Presentation.DTOs;

public record ContentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string AgeRating { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string GenreName { get; set; } = string.Empty;
    public int? DurationMin { get; set; }
    public int? SeasonsCount { get; set; }
}

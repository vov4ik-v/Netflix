namespace Netflix.Presentation.DTO;

public record ContentDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int ReleaseYear { get; init; }
    public string AgeRating { get; init; } = string.Empty;
    public string Language { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public double AverageRating { get; init; }
    public string ContentType { get; init; } = string.Empty;
    public string GenreName { get; init; } = string.Empty;
    public int? DurationMin { get; init; }
    public int? SeasonsCount { get; init; }
}

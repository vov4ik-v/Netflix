namespace Netflix.Presentation.DTO;

public record EpisodeDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public int SeasonNumber { get; init; }
    public int EpisodeNumber { get; init; }
    public int DurationMin { get; init; }
    public string Synopsis { get; init; } = string.Empty;
}
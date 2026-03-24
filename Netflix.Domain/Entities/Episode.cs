namespace Netflix.Domain.Entities;

public class Episode
{
    public int Id { get; init; }
    public int SeriesId { get; set; }
    public string Title { get; init; } = string.Empty;
    public int SeasonNumber { get; init; }
    public int EpisodeNumber { get; init; }
    public int DurationMin { get; init; }
    public string Synopsis { get; init; } = string.Empty;

    public Series? Series { get; init; }
}
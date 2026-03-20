namespace Netflix.Domain.Entities;

public class Episode
{
    public int Id { get; set; }
    public int SeriesId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int SeasonNumber { get; set; }
    public int EpisodeNumber { get; set; }
    public int DurationMin { get; set; }
    public string Synopsis { get; set; } = string.Empty;

    public Series Series { get; set; } = null!;
}
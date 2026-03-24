namespace Netflix.Domain.Entities;

public class Series : Content
{
    public int SeasonsCount { get; init; }

    public ICollection<Episode> Episodes { get; init; } = new List<Episode>();
}
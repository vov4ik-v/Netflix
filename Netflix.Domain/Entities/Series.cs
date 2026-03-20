namespace Netflix.Domain.Entities;

public class Series : Content
{
    public int SeasonsCount { get; set; }

    public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
}
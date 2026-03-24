namespace Netflix.Domain.Entities;

public class Genre
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;

    public ICollection<Content> Contents { get; init; } = new List<Content>();
}
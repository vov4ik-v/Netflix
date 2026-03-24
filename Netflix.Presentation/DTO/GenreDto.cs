namespace Netflix.Presentation.DTO;

public record GenreDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

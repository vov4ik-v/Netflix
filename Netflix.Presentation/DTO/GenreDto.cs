namespace Netflix.Presentation.DTOs;

public record GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

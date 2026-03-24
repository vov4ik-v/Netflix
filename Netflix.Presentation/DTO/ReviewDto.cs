namespace Netflix.Presentation.DTO;

public record ReviewDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Text { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

namespace Netflix.Presentation.DTO;

public record MyListItemDto
{
    public int Id { get; init; }
    public int ContentId { get; init; }
    public string ContentTitle { get; init; } = string.Empty;
    public DateTime AddedAt { get; init; }
}

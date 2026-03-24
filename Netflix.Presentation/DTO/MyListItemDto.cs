namespace Netflix.Presentation.DTOs;

public record MyListItemDto
{
    public int Id { get; set; }
    public int ContentId { get; set; }
    public string ContentTitle { get; set; } = string.Empty;
    public DateTime AddedAt { get; set; }
}

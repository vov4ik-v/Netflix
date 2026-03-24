namespace Netflix.Presentation.DTOs;

public record MyListDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<MyListItemDto> Items { get; set; } = new();
}

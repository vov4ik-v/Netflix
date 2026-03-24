namespace Netflix.Presentation.DTO;

public record MyListDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<MyListItemDto> Items { get; init; } = [];
}

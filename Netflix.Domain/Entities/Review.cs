namespace Netflix.Domain.Entities;

public class Review
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int ContentId { get; init; }
    public string Text { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }

    public User? User { get; init; }
    public Content? Content { get; init; }
}
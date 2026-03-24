namespace Netflix.Domain.Entities;

public class Rating
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int ContentId { get; init; }
    public int Score { get; init; }
    public DateTime CreatedAt { get; init; }

    public User User { get; init; } = null!;
    public Content Content { get; init; } = null!;
}
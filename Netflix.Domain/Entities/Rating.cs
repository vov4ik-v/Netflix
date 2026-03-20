namespace Netflix.Domain.Entities;

public class Rating
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ContentId { get; set; }
    public int Score { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public Content Content { get; set; } = null!;
}
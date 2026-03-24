namespace Netflix.Presentation.DTO;

public record RatingDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int Score { get; init; }
    public DateTime CreatedAt { get; init; }
}

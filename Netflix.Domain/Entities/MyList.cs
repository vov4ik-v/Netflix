namespace Netflix.Domain.Entities;

public class MyList
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public DateTime CreatedAt { get; init; }

    public User User { get; init; } = null!;
    public ICollection<MyListItem> Items { get; init; } = new List<MyListItem>();
}
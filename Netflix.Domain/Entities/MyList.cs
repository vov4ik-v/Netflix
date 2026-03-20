namespace Netflix.Domain.Entities;

public class MyList
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public ICollection<MyListItem> Items { get; set; } = new List<MyListItem>();
}
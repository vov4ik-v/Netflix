namespace Netflix.Domain.Entities;

public class MyListItem
{
    public int Id { get; set; }
    public int MyListId { get; set; }
    public int ContentId { get; set; }
    public DateTime AddedAt { get; set; }

    public MyList MyList { get; set; } = null!;
    public Content Content { get; set; } = null!;
}
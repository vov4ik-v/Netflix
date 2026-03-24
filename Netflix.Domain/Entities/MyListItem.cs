namespace Netflix.Domain.Entities;

public class MyListItem
{
    public int Id { get; init; }
    public int MyListId { get; init; }
    public int ContentId { get; init; }
    public DateTime AddedAt { get; init; }

    public MyList? MyList { get; init; }
    public Content? Content { get; init; }
}
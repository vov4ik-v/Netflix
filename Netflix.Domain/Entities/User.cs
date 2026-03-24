namespace Netflix.Domain.Entities;

public class User
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;

    public ICollection<Review> Reviews { get; init; } = new List<Review>();
    public ICollection<Rating> Ratings { get; init; } = new List<Rating>();
    public MyList? MyList { get; init; }
}
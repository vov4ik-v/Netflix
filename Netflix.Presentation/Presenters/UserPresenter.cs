using Netflix.Domain.Entities;
using Netflix.Presentation.Interfaces;

namespace Netflix.Presentation.Presenters;

public class UserPresenter : IUserPresenter
{
    public Task ShowUserProfileAsync(User user)
    {
        Console.WriteLine($"\n--- Profile ---");
        Console.WriteLine($"Name: {user.Name}");
        Console.WriteLine($"Email: {user.Email}");
        Console.WriteLine($"---------------");
        return Task.CompletedTask;
    }

    public Task ShowMyListAsync(MyList myList)
    {
        Console.WriteLine($"\n--- My List (User {myList.UserId}) ---");
        if (myList.Items == null || !myList.Items.Any())
        {
            Console.WriteLine("Your list is empty.");
        }
        else
        {
            foreach (var item in myList.Items)
            {
                // In a real app we'd load the content title. Assuming it might be null if not eagerly loaded.
                var title = item.Content?.Title ?? $"Content ID {item.ContentId}";
                Console.WriteLine($"- {title} (Added: {item.AddedAt.ToShortDateString()})");
            }
        }
        Console.WriteLine("----------------");
        return Task.CompletedTask;
    }
}

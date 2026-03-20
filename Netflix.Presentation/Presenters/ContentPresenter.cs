using Netflix.Domain.Entities;
using Netflix.Presentation.Interfaces;

namespace Netflix.Presentation.Presenters;

public class ContentPresenter : IContentPresenter
{
    public Task ShowContentDetailsAsync(Content content)
    {
        Console.WriteLine($"\n=== {content.Title} ===");
        Console.WriteLine($"Type: {content.GetType().Name}");
        Console.WriteLine($"Year: {content.ReleaseYear} | Rating: {content.AgeRating} | Avg: {content.AverageRating}/5");
        Console.WriteLine($"Desc: {content.Description}");
        
        if (content is Movie m)
            Console.WriteLine($"Duration: {m.DurationMin} min");
        else if (content is Series s)
            Console.WriteLine($"Seasons: {s.SeasonsCount}");

        Console.WriteLine("=======================\n");
        return Task.CompletedTask;
    }

    public Task ShowReviewsAsync(IEnumerable<Review> reviews)
    {
        Console.WriteLine("\n--- Reviews ---");
        foreach (var r in reviews)
        {
            Console.WriteLine($"[User {r.UserId}]: {r.Text} ({r.CreatedAt.ToShortDateString()})");
        }
        Console.WriteLine("---------------");
        return Task.CompletedTask;
    }

    public Task ShowRatingsAsync(IEnumerable<Rating> ratings)
    {
        Console.WriteLine("\n--- Ratings ---");
        foreach (var r in ratings)
        {
            Console.WriteLine($"[User {r.UserId}] voted {r.Score}/10");
        }
        Console.WriteLine("---------------");
        return Task.CompletedTask;
    }
}

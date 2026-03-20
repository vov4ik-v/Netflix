using Netflix.Domain.Entities;
using Netflix.Presentation.Interfaces;

namespace Netflix.Presentation.Presenters;

public class CatalogPresenter : ICatalogPresenter
{
    public Task ShowContentAsync(IEnumerable<Content> contents)
    {
        Console.WriteLine("\n--- Netflix Catalog ---");
        foreach (var c in contents)
        {
            Console.WriteLine($"[{c.Id}] {c.Title} ({c.ReleaseYear}) - Rating: {c.AverageRating}");
        }
        Console.WriteLine("-----------------------");
        return Task.CompletedTask;
    }

    public Task SearchByCategoryAsync(string category)
    {
        Console.WriteLine($"\nSearching by category '{category}'...");
        // BLL handles the actual searching, the Presenter just shows the UI prompt or result.
        // We'll pass the UI interaction back to the caller (ConsoleMenu) who provides the Content list.
        return Task.CompletedTask;
    }

    public Task FilterContentAsync(int? genreId, int? releaseYear, string? language, double? minRating)
    {
        Console.WriteLine("\nFiltering catalog...");
        return Task.CompletedTask;
    }
}

using Netflix.Domain.Entities;

namespace Netflix.Presentation.Interfaces;

public interface ICatalogPresenter
{
    Task ShowContentAsync(IEnumerable<Content> contents);
    Task SearchByCategoryAsync(string category);
    Task FilterContentAsync(int? genreId, int? releaseYear, string? language, double? minRating);
}
using Netflix.Domain.Entities;

namespace Netflix.BLL.Interfaces;

public interface ICatalogService
{
    Task<IEnumerable<Content>> GetAllContentAsync();
    Task<IEnumerable<Content>> SearchContentByTitleAsync(string title);
    Task<IEnumerable<Content>> GetContentByGenreAsync(string genreName);
    Task<Content?> GetContentDetailsAsync(int id);
    Task<IEnumerable<Genre>> GetAllGenresAsync();
    Task<IEnumerable<Review>> GetReviewsForContentAsync(int contentId);
    Task<IEnumerable<Rating>> GetRatingsForContentAsync(int contentId);
}
using Netflix.BusinessLogic.Interfaces;
using Netflix.DataAccess.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.BusinessLogic.Services;

public class CatalogService(
    IContentRepository contentRepository,
    IGenreRepository genreRepository,
    IReviewRepository reviewRepository,
    IRatingRepository ratingRepository)
    : ICatalogService
{
    public async Task<IEnumerable<Content>> GetAllContentAsync()
    {
        return await contentRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Content>> SearchContentByTitleAsync(string title)
    {
        var all = await contentRepository.GetAllAsync();
        return all.Where(c => c.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Content>> GetContentByGenreAsync(string genreName)
    {
        var all = await contentRepository.GetAllAsync();
        return all.Where(c => c.Genre != null && c.Genre.Name.Equals(genreName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Content?> GetContentDetailsAsync(int id)
    {
        return await contentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Genre>> GetAllGenresAsync()
    {
        return await genreRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsForContentAsync(int contentId)
    {
        return await reviewRepository.GetByContentIdAsync(contentId);
    }

    public async Task<IEnumerable<Rating>> GetRatingsForContentAsync(int contentId)
    {
        return await ratingRepository.GetByContentIdAsync(contentId);
    }
}

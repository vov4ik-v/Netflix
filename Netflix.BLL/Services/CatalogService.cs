using Netflix.BLL.Interfaces;
using Netflix.DAL.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.BLL.Services;

public class CatalogService : ICatalogService
{
    private readonly IContentRepository _contentRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IRatingRepository _ratingRepository;

    public CatalogService(IContentRepository contentRepository, IGenreRepository genreRepository, IReviewRepository reviewRepository, IRatingRepository ratingRepository)
    {
        _contentRepository = contentRepository;
        _genreRepository = genreRepository;
        _reviewRepository = reviewRepository;
        _ratingRepository = ratingRepository;
    }

    public async Task<IEnumerable<Content>> GetAllContentAsync()
    {
        return await _contentRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Content>> SearchContentByTitleAsync(string title)
    {
        var all = await _contentRepository.GetAllAsync();
        return all.Where(c => c.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Content>> GetContentByGenreAsync(string genreName)
    {
        var all = await _contentRepository.GetAllAsync();
        // Here we ideally want to join with Genre or assume Genre is loaded. 
        // In this lab, we might need a specific repository method, but let's filter purely in memory if Genre navigation is populated
        return all.Where(c => c.Genre != null && c.Genre.Name.Equals(genreName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Content?> GetContentDetailsAsync(int id)
    {
        return await _contentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Genre>> GetAllGenresAsync()
    {
        return await _genreRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsForContentAsync(int contentId)
    {
        return await _reviewRepository.GetByContentIdAsync(contentId);
    }

    public async Task<IEnumerable<Rating>> GetRatingsForContentAsync(int contentId)
    {
        return await _ratingRepository.GetByContentIdAsync(contentId);
    }
}

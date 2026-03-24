using Netflix.Domain.Entities;

namespace Netflix.DataAccess.Interfaces;

public interface IReviewRepository
{
    Task<List<Review>> GetByContentIdAsync(int contentId);
    Task AddAsync(Review review);
    Task AddRangeAsync(IEnumerable<Review> reviews);
    Task SaveChangesAsync();
}
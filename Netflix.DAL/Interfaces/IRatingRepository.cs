using Netflix.Domain.Entities;

namespace Netflix.DAL.Interfaces;

public interface IRatingRepository
{
    Task<List<Rating>> GetByContentIdAsync(int contentId);
    Task AddAsync(Rating rating);
    Task AddRangeAsync(IEnumerable<Rating> ratings);
    Task SaveChangesAsync();
}
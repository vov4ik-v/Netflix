using Microsoft.EntityFrameworkCore;
using Netflix.DataAccess.Data;
using Netflix.DataAccess.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DataAccess.Repositories;

public class RatingRepository(NetflixDbContext context) : IRatingRepository
{
    public async Task<List<Rating>> GetByContentIdAsync(int contentId)
    {
        return await context.Ratings.Where(r => r.ContentId == contentId).ToListAsync();
    }

    public async Task AddAsync(Rating rating)
    {
        await context.Ratings.AddAsync(rating);
    }

    public async Task AddRangeAsync(IEnumerable<Rating> ratings)
    {
        await context.Ratings.AddRangeAsync(ratings);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using Netflix.DataAccess.Data;
using Netflix.DataAccess.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DataAccess.Repositories;

public class ReviewRepository(NetflixDbContext context) : IReviewRepository
{
    public async Task<List<Review>> GetByContentIdAsync(int contentId)
    {
        return await context.Reviews.Where(r => r.ContentId == contentId).ToListAsync();
    }

    public async Task AddAsync(Review review)
    {
        await context.Reviews.AddAsync(review);
    }

    public async Task AddRangeAsync(IEnumerable<Review> reviews)
    {
        await context.Reviews.AddRangeAsync(reviews);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
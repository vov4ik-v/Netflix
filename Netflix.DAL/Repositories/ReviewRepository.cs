using Microsoft.EntityFrameworkCore;
using Netflix.DAL.Data;
using Netflix.DAL.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DAL.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly NetflixDbContext _context;

    public ReviewRepository(NetflixDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetByContentIdAsync(int contentId)
    {
        return await _context.Reviews.Where(r => r.ContentId == contentId).ToListAsync();
    }

    public async Task AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
    }

    public async Task AddRangeAsync(IEnumerable<Review> reviews)
    {
        await _context.Reviews.AddRangeAsync(reviews);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
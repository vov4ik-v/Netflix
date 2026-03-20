using Microsoft.EntityFrameworkCore;
using Netflix.DAL.Data;
using Netflix.DAL.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DAL.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly NetflixDbContext _context;

    public RatingRepository(NetflixDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rating>> GetByContentIdAsync(int contentId)
    {
        return await _context.Ratings.Where(r => r.ContentId == contentId).ToListAsync();
    }

    public async Task AddAsync(Rating rating)
    {
        await _context.Ratings.AddAsync(rating);
    }

    public async Task AddRangeAsync(IEnumerable<Rating> ratings)
    {
        await _context.Ratings.AddRangeAsync(ratings);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
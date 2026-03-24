using Microsoft.EntityFrameworkCore;
using Netflix.DataAccess.Data;
using Netflix.DataAccess.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DataAccess.Repositories;

public class ContentRepository(NetflixDbContext context) : IContentRepository
{
    public async Task<Content?> GetByIdAsync(int id)
    {
        return await context.Contents
            .Include(c => c.Genre)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Content>> GetAllAsync()
    {
        return await context.Contents
            .Include(c => c.Genre)
            .ToListAsync();
    }

    public async Task AddAsync(Content content)
    {
        await context.Contents.AddAsync(content);
    }

    public async Task AddRangeAsync(IEnumerable<Content> contents)
    {
        await context.Contents.AddRangeAsync(contents);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
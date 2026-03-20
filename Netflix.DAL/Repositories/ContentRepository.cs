using Microsoft.EntityFrameworkCore;
using Netflix.DAL.Data;
using Netflix.DAL.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DAL.Repositories;

public class ContentRepository : IContentRepository
{
    private readonly NetflixDbContext _context;

    public ContentRepository(NetflixDbContext context)
    {
        _context = context;
    }

    public async Task<Content?> GetByIdAsync(int id)
    {
        return await _context.Contents.FindAsync(id);
    }

    public async Task<List<Content>> GetAllAsync()
    {
        return await _context.Contents.ToListAsync();
    }

    public async Task AddAsync(Content content)
    {
        await _context.Contents.AddAsync(content);
    }

    public async Task AddRangeAsync(IEnumerable<Content> contents)
    {
        await _context.Contents.AddRangeAsync(contents);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
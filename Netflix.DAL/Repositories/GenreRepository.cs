using Microsoft.EntityFrameworkCore;
using Netflix.DAL.Data;
using Netflix.DAL.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DAL.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly NetflixDbContext _context;

    public GenreRepository(NetflixDbContext context)
    {
        _context = context;
    }

    public async Task<Genre?> GetByNameAsync(string name)
    {
        return await _context.Genres.FirstOrDefaultAsync(g => g.Name == name);
    }

    public async Task<List<Genre>> GetAllAsync()
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task AddAsync(Genre genre)
    {
        await _context.Genres.AddAsync(genre);
    }

    public async Task AddRangeAsync(IEnumerable<Genre> genres)
    {
        await _context.Genres.AddRangeAsync(genres);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
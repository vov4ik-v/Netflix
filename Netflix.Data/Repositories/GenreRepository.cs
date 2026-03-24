using Microsoft.EntityFrameworkCore;
using Netflix.DataAccess.Data;
using Netflix.DataAccess.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DataAccess.Repositories;

public class GenreRepository(NetflixDbContext context) : IGenreRepository
{
    public async Task<Genre?> GetByNameAsync(string name)
    {
        return await context.Genres.FirstOrDefaultAsync(g => g.Name == name);
    }

    public async Task<List<Genre>> GetAllAsync()
    {
        return await context.Genres.ToListAsync();
    }

    public async Task AddAsync(Genre genre)
    {
        await context.Genres.AddAsync(genre);
    }

    public async Task AddRangeAsync(IEnumerable<Genre> genres)
    {
        await context.Genres.AddRangeAsync(genres);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
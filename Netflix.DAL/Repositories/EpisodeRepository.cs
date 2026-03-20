using Microsoft.EntityFrameworkCore;
using Netflix.DAL.Data;
using Netflix.DAL.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DAL.Repositories;

public class EpisodeRepository : IEpisodeRepository
{
    private readonly NetflixDbContext _context;

    public EpisodeRepository(NetflixDbContext context)
    {
        _context = context;
    }

    public async Task<List<Episode>> GetBySeriesIdAsync(int seriesId)
    {
        return await _context.Episodes.Where(e => e.SeriesId == seriesId).ToListAsync();
    }

    public async Task AddAsync(Episode episode)
    {
        await _context.Episodes.AddAsync(episode);
    }

    public async Task AddRangeAsync(IEnumerable<Episode> episodes)
    {
        await _context.Episodes.AddRangeAsync(episodes);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
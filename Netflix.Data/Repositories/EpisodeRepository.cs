using Microsoft.EntityFrameworkCore;
using Netflix.DataAccess.Data;
using Netflix.DataAccess.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DataAccess.Repositories;

public class EpisodeRepository(NetflixDbContext context) : IEpisodeRepository
{
    public async Task<List<Episode>> GetBySeriesIdAsync(int seriesId)
    {
        return await context.Episodes.Where(e => e.SeriesId == seriesId).ToListAsync();
    }

    public async Task AddAsync(Episode episode)
    {
        await context.Episodes.AddAsync(episode);
    }

    public async Task AddRangeAsync(IEnumerable<Episode> episodes)
    {
        await context.Episodes.AddRangeAsync(episodes);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
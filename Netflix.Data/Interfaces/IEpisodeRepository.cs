using Netflix.Domain.Entities;

namespace Netflix.DataAccess.Interfaces;

public interface IEpisodeRepository
{
    Task<List<Episode>> GetBySeriesIdAsync(int seriesId);
    Task AddAsync(Episode episode);
    Task AddRangeAsync(IEnumerable<Episode> episodes);
    Task SaveChangesAsync();
}
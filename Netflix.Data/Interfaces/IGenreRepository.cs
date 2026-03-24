using Netflix.Domain.Entities;

namespace Netflix.DataAccess.Interfaces;

public interface IGenreRepository
{
    Task<Genre?> GetByNameAsync(string name);
    Task<List<Genre>> GetAllAsync();
    Task AddAsync(Genre genre);
    Task AddRangeAsync(IEnumerable<Genre> genres);
    Task SaveChangesAsync();
}
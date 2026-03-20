using Netflix.Domain.Entities;

namespace Netflix.DAL.Interfaces;

public interface IContentRepository
{
    Task<Content?> GetByIdAsync(int id);
    Task<List<Content>> GetAllAsync();
    Task AddAsync(Content content);
    Task AddRangeAsync(IEnumerable<Content> contents);
    Task SaveChangesAsync();
}
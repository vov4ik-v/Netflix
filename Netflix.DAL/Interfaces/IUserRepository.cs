using Netflix.Domain.Entities;

namespace Netflix.DAL.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetAllAsync();
    Task AddAsync(User user);
    Task AddRangeAsync(IEnumerable<User> users);
    Task SaveChangesAsync();
}
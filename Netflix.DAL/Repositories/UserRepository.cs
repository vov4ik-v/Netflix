using Microsoft.EntityFrameworkCore;
using Netflix.DAL.Data;
using Netflix.DAL.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NetflixDbContext _context;

    public UserRepository(NetflixDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task AddRangeAsync(IEnumerable<User> users)
    {
        await _context.Users.AddRangeAsync(users);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
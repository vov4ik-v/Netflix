using Microsoft.EntityFrameworkCore;
using Netflix.DAL.Data;
using Netflix.DAL.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DAL.Repositories;

public class MyListRepository : IMyListRepository
{
    private readonly NetflixDbContext _context;

    public MyListRepository(NetflixDbContext context)
    {
        _context = context;
    }

    public async Task<MyList?> GetByUserIdAsync(int userId)
    {
        return await _context.MyLists
            .Include(m => m.Items)
            .FirstOrDefaultAsync(m => m.UserId == userId);
    }

    public async Task AddAsync(MyList myList)
    {
        await _context.MyLists.AddAsync(myList);
    }

    public async Task AddMyListItemAsync(MyListItem item)
    {
        await _context.MyListItems.AddAsync(item);
    }

    public async Task AddMyListItemsRangeAsync(IEnumerable<MyListItem> items)
    {
        await _context.MyListItems.AddRangeAsync(items);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
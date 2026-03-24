using Microsoft.EntityFrameworkCore;
using Netflix.DataAccess.Data;
using Netflix.DataAccess.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DataAccess.Repositories;

public class MyListRepository(NetflixDbContext context) : IMyListRepository
{
    public async Task<MyList?> GetByUserIdAsync(int userId)
    {
        return await context.MyLists
            .Include(m => m.Items)
            .FirstOrDefaultAsync(m => m.UserId == userId);
    }

    public async Task AddAsync(MyList myList)
    {
        await context.MyLists.AddAsync(myList);
    }

    public async Task AddMyListItemAsync(MyListItem item)
    {
        await context.MyListItems.AddAsync(item);
    }

    public async Task AddMyListItemsRangeAsync(IEnumerable<MyListItem> items)
    {
        await context.MyListItems.AddRangeAsync(items);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
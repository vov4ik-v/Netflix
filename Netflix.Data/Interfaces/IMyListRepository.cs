using Netflix.Domain.Entities;

namespace Netflix.DataAccess.Interfaces;

public interface IMyListRepository
{
    Task<MyList?> GetByUserIdAsync(int userId);
    Task AddAsync(MyList myList);
    Task AddMyListItemAsync(MyListItem item);
    Task AddMyListItemsRangeAsync(IEnumerable<MyListItem> items);
    Task SaveChangesAsync();
}
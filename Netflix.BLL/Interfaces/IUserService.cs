using Netflix.Domain.Entities;

namespace Netflix.BLL.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<MyList?> GetUserMyListAsync(int userId);
}

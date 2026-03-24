using Netflix.Domain.Entities;

namespace Netflix.BusinessLogic.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<MyList?> GetUserMyListAsync(int userId);
}

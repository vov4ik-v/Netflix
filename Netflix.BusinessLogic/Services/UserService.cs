using Netflix.BusinessLogic.Interfaces;
using Netflix.DataAccess.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.BusinessLogic.Services;

public class UserService(IUserRepository userRepository, IMyListRepository myListRepository)
    : IUserService
{
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var users = await userRepository.GetAllAsync();
        return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<MyList?> GetUserMyListAsync(int userId)
    {
        return await myListRepository.GetByUserIdAsync(userId);
    }
}

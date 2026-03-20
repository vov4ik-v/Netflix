using Netflix.BLL.Interfaces;
using Netflix.DAL.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMyListRepository _myListRepository;

    public UserService(IUserRepository userRepository, IMyListRepository myListRepository)
    {
        _userRepository = userRepository;
        _myListRepository = myListRepository;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var users = await _userRepository.GetAllAsync();
        return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<MyList?> GetUserMyListAsync(int userId)
    {
        return await _myListRepository.GetByUserIdAsync(userId);
    }
}

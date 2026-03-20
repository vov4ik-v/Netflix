using Netflix.Domain.Entities;

namespace Netflix.Presentation.Interfaces;

public interface IUserPresenter
{
    Task ShowUserProfileAsync(User user);
    Task ShowMyListAsync(MyList myList);
}
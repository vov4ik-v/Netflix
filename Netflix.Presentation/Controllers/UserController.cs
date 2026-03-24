using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Netflix.BusinessLogic.Interfaces;
using Netflix.Presentation.DTO;

namespace Netflix.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, IMapper mapper) : ControllerBase
{
    [HttpGet("{email}")]
    public async Task<ActionResult<UserDto>> GetByEmail(string email)
    {
        var user = await userService.GetUserByEmailAsync(email);
        if (user == null)
            return NotFound();

        return Ok(mapper.Map<UserDto>(user));
    }

    [HttpGet("{userId:int}/mylist")]
    public async Task<ActionResult<MyListDto>> GetMyList(int userId)
    {
        var myList = await userService.GetUserMyListAsync(userId);
        if (myList == null)
            return NotFound();

        return Ok(mapper.Map<MyListDto>(myList));
    }
}

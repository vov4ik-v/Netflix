using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Netflix.BusinessLogic.Interfaces;
using Netflix.Presentation.DTOs;

namespace Netflix.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<UserDto>> GetByEmail(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            return NotFound();

        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpGet("{userId:int}/mylist")]
    public async Task<ActionResult<MyListDto>> GetMyList(int userId)
    {
        var myList = await _userService.GetUserMyListAsync(userId);
        if (myList == null)
            return NotFound();

        return Ok(_mapper.Map<MyListDto>(myList));
    }
}

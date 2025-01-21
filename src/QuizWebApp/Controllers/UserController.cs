using Microsoft.AspNetCore.Mvc;
using QuizWebApp.DTOs;
using QuizWebApp.Interfaces;

namespace QuizWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetUserList()
    {
        var userList = await _userService.GetUserListAsync();
        return Ok(userList);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }

    [HttpGet("email")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        return Ok(user);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto UserCreateDto)
    {
        if (!ModelState.IsValid)
        {
            // Todo change
            return BadRequest("Bad request");
        }

        var newUser = await _userService.CreateUserAsync(UserCreateDto);
        return CreatedAtAction(nameof(GetUserByEmail), new { email = UserCreateDto.Email }, newUser);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        await _userService.DeleteUserAsync(id);
        return Ok(new { message = "User deleted." });
    }
}

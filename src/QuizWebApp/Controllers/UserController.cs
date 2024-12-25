using Microsoft.AspNetCore.Mvc;
using QuizWebApp.DTOs;
using QuizWebApp.Exceptions;
using QuizWebApp.Services;

namespace QuizWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(UserService userService) : ControllerBase
{
    private readonly UserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

    [HttpGet("list")]
    public async Task<IActionResult> GetUserList()
    {
        try
        {
            var userList = await _userService.GetUserListAsync();
            return Ok(userList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("email")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(user);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto UserCreateDto)
    {
        try
        {
            var newUser = await _userService.CreateUserAsync(UserCreateDto);
            return CreatedAtAction(nameof(GetUserByEmail), new { email = UserCreateDto.Email }, newUser);
        }
        catch (AlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return Ok(new { message = "User deleted." });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using QuizWebApp.DTOs;
using QuizWebApp.Exceptions;
using QuizWebApp.Services;

namespace QuizWebApp.Controllers;

[ApiController]
[Route("users")]
public class UserController(UserService userService) : ControllerBase
{
    UserService _userService => userService;

    [HttpGet("")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { message = "User not found."});
            }

            return Ok(user);
        }
        catch (ApplicationException ex) 
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        try {
            var newUser = await _userService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(GetUserByEmail), new { email = createUserDto.Email }, newUser);
        }
        catch (AlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
namespace QuizWebApp.Controllers;

using Microsoft.AspNetCore.Mvc;
using QuizWebApp.DTOs;
using QuizWebApp.Interfaces;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        try
        {
            var loginResponse = await _authService.Login(loginDto);
            return Ok(loginResponse);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(401, new { message = ex.Message });
        }
    }

}

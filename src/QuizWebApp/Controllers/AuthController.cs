namespace QuizWebApp.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using QuizWebApp.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using QuizWebApp.DTOs;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;

    public AuthController(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto model)
    {
        if (model is { Email: "demo", Password: "password"})
        {
            var token = GenerateAccessToken(model.Email);
            return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token)});
        }

        return Unauthorized("Invalid Credentials");
    }

    private JwtSecurityToken GenerateAccessToken(string userName)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userName),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secretkey)),
            SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}

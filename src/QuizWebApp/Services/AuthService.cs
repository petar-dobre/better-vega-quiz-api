using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuizWebApp.Configuration;
using QuizWebApp.DTOs;
using QuizWebApp.Exceptions;
using QuizWebApp.Repositories;
using QuizWebApp.Interfaces;

namespace QuizWebApp.Services;

public class AuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    public AuthService(IOptions<JwtSettings> jwtSettings, IPasswordHasher passwordHasher, IUserRepository userRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Login failed due to invalid credentials.");
        }

        var passwordMatched = _passwordHasher.VerifyHashedPassword(user.Password, loginRequestDto.Password);
        if (passwordMatched == false)
        {
            throw new UnauthorizedAccessException("Login failed due to invalid credentials.");
        }

        var token = GenerateAccessToken(user.Email);

        return new LoginResponseDto{
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token)
        };    
    }

    private JwtSecurityToken GenerateAccessToken(string email)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, email),
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
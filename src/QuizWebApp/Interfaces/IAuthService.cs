using QuizWebApp.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace QuizWebApp.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
}
   
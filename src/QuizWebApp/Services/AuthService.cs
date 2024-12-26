using QuizWebApp.DTOs;
using QuizWebApp.Exceptions;
using QuizWebApp.Repositories;

namespace QuizWebApp.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;

    public AuthService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Login failed due to invalid credentials.");
        }

        
    }
}
using System.Collections.Generic;
using QuizWebApp.DTOs;

namespace QuizWebApp.Interfaces;

public interface IUserService
{
    Task<List<UserResponseDto>> GetUserListAsync();

    Task<UserResponseDto> GetUserByIdAsync(int id);

    Task<UserResponseDto> GetUserByEmailAsync(string email);

    Task<UserResponseDto> CreateUserAsync(UserCreateDto userCreateDto);

    Task DeleteUserAsync(int id);
}
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using QuizWebApp.Domain.Models;
using QuizWebApp.DTOs;
using QuizWebApp.Exceptions;
using QuizWebApp.Repositories;

namespace QuizWebApp.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;

    public UserService(UserRepository userRepository, PasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<List<UserResponseDto>> GetUserListAsync()
    {
        var userList = await _userRepository.GetUserListAsync();

        var responseItemList = new List<UserResponseDto>();
        foreach (User user in userList)
        {
            responseItemList.Add(UserResponseDto.CreateFromModel(user));
        }

        return responseItemList;
    }

    public async Task<UserResponseDto> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with id {id} not found.");
        }

        var userResponseDto = UserResponseDto.CreateFromModel(user);

        return userResponseDto;
    }

    public async Task<UserResponseDto> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException($"User with email {email} not found.");
        }

        var userResponseDto = UserResponseDto.CreateFromModel(user);

        return userResponseDto;
    }

    public async Task<UserResponseDto> CreateUserAsync(UserCreateDto userCreateDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(userCreateDto.Email);
        if (existingUser != null)
        {
            throw new AlreadyExistsException($"A user with email {userCreateDto.Email} already exists.");
        }

        var newUser = User.CreateFromDto(userCreateDto);

        var hashedPassword = _passwordHasher.HashPassword(userCreateDto.Password);
        newUser.UpdatePassword(hashedPassword);

        _userRepository.CreateUser(newUser);

        var userResponseDto = UserResponseDto.CreateFromModel(newUser);
        return userResponseDto;
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with id {id} not found.");
        }

        _userRepository.DeleteUserAsync(user);
    }
}
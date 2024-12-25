using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using QuizWebApp.Domain.Models;
using QuizWebApp.DTOs;
using QuizWebApp.Exceptions;
using QuizWebApp.Repositories;

namespace QuizWebApp.Services;

public class UserService(UserRepository userRepo)
{
    private readonly UserRepository _repo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));

    public async Task<List<UserResponseDto>> GetUserListAsync()
    {
        var userList = await _repo.GetUserListAsync();

        var responseItemList = new List<UserResponseDto>();
        foreach (User user in userList)
        {
            responseItemList.Add(UserResponseDto.CreateFromModel(user));
        }

        return responseItemList;
    }

    public async Task<UserResponseDto> GetUserByIdAsync(int id)
    {
        var user = await _repo.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with id {id} not found.");
        }

        var userResponseDto = UserResponseDto.CreateFromModel(user);

        return userResponseDto;
    }

    public async Task<UserResponseDto> GetUserByEmailAsync(string email)
    {
        var user = await _repo.GetByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException($"User with email {email} not found.");
        }

        var userResponseDto = UserResponseDto.CreateFromModel(user);

        return userResponseDto;
    }

    public async Task<UserResponseDto> CreateUserAsync(UserCreateDto UserCreateDto)
    {
        var existingUser = await _repo.GetByEmailAsync(UserCreateDto.Email);
        if (existingUser != null)
        {
            throw new AlreadyExistsException($"A user with email {UserCreateDto.Email} already exists.");
        }

        var newUser = User.CreateFromDto(UserCreateDto);

        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: UserCreateDto.Password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        ));

        newUser.UpdatePassword(hashedPassword);

        _repo.CreateUser(newUser);

        var userResponseDto = UserResponseDto.CreateFromModel(newUser);
        return userResponseDto;
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _repo.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with id {id} not found.");
        }

        _repo.DeleteUserAsync(user);
    }
}
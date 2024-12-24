using QuizWebApp.Domain.Models;
using QuizWebApp.DTOs;
using QuizWebApp.Exceptions;
using QuizWebApp.Repositories;

namespace QuizWebApp.Services;

public class UserService(UserRepository userRepo)
{
    UserRepository _repo => userRepo;

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            return await _repo.GetByEmailAsync(email);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Unable to fetch user data.", ex);
        }
    }

    public async Task<User?> CreateUserAsync(CreateUserDto createUserDto)
    {
        try 
        {
            var newUser = User.CreateFromDto(createUserDto);

            var existingUser = await _repo.GetByEmailAsync(newUser.Email);
            if (existingUser != null)
            {
                throw new AlreadyExistsException($"A user with email {newUser.Email} already exists.");
            }

            _repo.CreateUser(newUser);
        
            return newUser;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Unable to create a user.", ex);
        }

    }
}
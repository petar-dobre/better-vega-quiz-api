using Microsoft.AspNetCore.Identity;
using QuizWebApp.DTOs;

namespace QuizWebApp.Domain.Models;

public class User : IdentityUser
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public bool IsAdmin { get; private set; }

    public User(string firstName, string lastName, string email, string password, bool isAdmin = false)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        IsAdmin = isAdmin;
    }

    public void GrantAdminRights()
    {
        IsAdmin = true;
    }

    public void RevokeAdminRights()
    {
        IsAdmin = false;
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }

    public void UpdatePassword(string password)
    {
        PasswordHash = password;
    }

    public static User CreateFromDto(UserCreateDto UserCreateDto)
    {
        return new User(
            firstName: UserCreateDto.FirstName,
            lastName: UserCreateDto.LastName,
            email: UserCreateDto.Email,
            password: UserCreateDto.Password
        );
    }
}

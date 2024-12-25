using System.ComponentModel.DataAnnotations;
using QuizWebApp.Domain.Models;

namespace QuizWebApp.DTOs;

public class UserCreateDto
{
    [Required(ErrorMessage = "First name is required")]
    public required string FirstName { get; set; }
    [Required(ErrorMessage = "Last name is required")]
    public required string LastName { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}

public class UserResponseDto
{
    [Required(ErrorMessage = "First name is required")]
    public required string FirstName { get; set; }
    [Required(ErrorMessage = "Last name is required")]
    public required string LastName { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public required string Email { get; set; }
    [Required(ErrorMessage = "IsAdmin is required")]
    public required bool IsAdmin { get; set; }

    public static UserResponseDto CreateFromModel(User user)
    {
        return new UserResponseDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsAdmin = user.IsAdmin
        };
    }
}
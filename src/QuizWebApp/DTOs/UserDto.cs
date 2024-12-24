using System.ComponentModel.DataAnnotations;

namespace QuizWebApp.DTOs;

public class CreateUserDto
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
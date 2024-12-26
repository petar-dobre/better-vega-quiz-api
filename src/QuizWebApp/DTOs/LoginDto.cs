namespace QuizWebApp.DTOs;

using System.ComponentModel.DataAnnotations;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}

public class LoginResponseDto
{
    [Required(ErrorMessage = "AccessToken is required")]
    public required string AccessToken { get; set; }
}
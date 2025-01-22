using Microsoft.AspNetCore.Identity;

namespace QuizWebApp.Domain.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsAdmin { get; set; }
}

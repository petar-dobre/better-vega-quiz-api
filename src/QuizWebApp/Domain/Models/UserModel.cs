namespace QuizWebApp.Domain.Models;

public class User
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public bool IsAdmin { get; private set; }

    public User(string firstName, string lastName, string email, string passwordHash, bool isAdmin = false)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        IsAdmin = isAdmin;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
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
}
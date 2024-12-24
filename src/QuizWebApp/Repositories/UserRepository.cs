namespace QuizWebApp.Repositories;

using Microsoft.EntityFrameworkCore;
using QuizWebApp.Configuration;
using QuizWebApp.Domain.Models;

public class UserRepository(AppDbContext db)
{
    AppDbContext _db => db;

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _db.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();
    }

    public void CreateUser(User user)
    {
        _db.Users.Add(user);
        _db.SaveChanges();
    }
}
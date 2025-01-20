using Microsoft.EntityFrameworkCore;
using QuizWebApp.Configuration;
using QuizWebApp.Domain.Models;
using QuizWebApp.Interfaces;

namespace QuizWebApp.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<User>> GetUserListAsync()
    {
        return await _db.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _db.Users
            .FindAsync(id);
    }

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

    public void DeleteUserAsync(User user)
    {
        _db.Users.Remove(user);
        _db.SaveChanges();
    }
}
using QuizWebApp.Domain.Models;

namespace QuizWebApp.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetUserListAsync();

    Task<User> GetUserByIdAsync(int id);

    Task<User> GetByEmailAsync(string email);

    void CreateUser(User user);

    void DeleteUserAsync(User user);
}
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string username, string password);
        Task<(string?, Guid? userId)> LoginAsync(string username, string password);
        Task LogoutAsync(string username);
    }
}

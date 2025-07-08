using JwtAuthentication.Entity;
using JwtAuthentication.Models;

namespace JwtAuthentication.Services
{
    public interface IUserService
    {
        Task<User?> RegisterAsync(UserDTO dto);
        Task<string> LoginAsync(UserDTO dto);
    }
}

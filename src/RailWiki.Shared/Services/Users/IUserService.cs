using System.Threading.Tasks;
using RailWiki.Shared.Entities.Users;

namespace RailWiki.Shared.Services.Users
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(RegisterUserRequest request);
    }
}

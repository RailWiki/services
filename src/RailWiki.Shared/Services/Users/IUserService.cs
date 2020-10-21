using System.Threading.Tasks;
using RailWiki.Shared.Entities.Users;
using RailWiki.Shared.Models.Users;

namespace RailWiki.Shared.Services.Users
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<GetUserModel> GetUserBySlugAsync(string slug);

        Task<User> RegisterUserAsync(RegisterUserRequest request);
        Task CreateUserAsync(User user);
    }
}

using UserService.DataLayer.Models;

namespace UserService.DataLayer.Repository
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User> GetUserByEmail(string email);
    }
}

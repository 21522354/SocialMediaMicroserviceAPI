using UserService.DataLayer.Models;

namespace UserService.DataLayer.Repository
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User> GetUserByEmail(string email);
        Task<User> SignUp(string email, string password);
        Task<User> SignIn(string email, string password);
        Task<User> SignInWithFB(string fbId);
        Task<User> GetByNickName(string nickName);  
        Task<IEnumerable<User>> GetRelateNickNameUser(string nickName); 
        Task<IEnumerable<User>> GetListUserFollower(Guid userId);
        Task<IEnumerable<User>> GetListUserFollowing(Guid userId);  
        Task<IEnumerable<User>> GetAllUsers();
        Task ChangePassword(Guid userId, string oldPassword,string newPassword);
        Task SetNameAndAvatar(Guid userId, string newName, string avatar);  
    }
}

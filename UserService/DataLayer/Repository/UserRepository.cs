using Microsoft.EntityFrameworkCore;
using UserService.DataLayer.Models;
using UserService.Error;

namespace UserService.DataLayer.Repository
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        private readonly UserDBContext _context;
        public UserRepository(UserDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task ChangePassword(Guid userId, string oldPassword,string newPassword)
        {
            var user = await _context.Users.FindAsync(userId); 
            if (user == null)
            {
                throw new BadHttpRequestException("User not found");
            }
            if(user.Password != oldPassword)
            {
                throw new BadHttpRequestException("Old password does not correct");
            }
            user.Password = newPassword;
            await _context.SaveChangesAsync();  
        }
        public async Task SetNameAndAvatar(Guid userId, string newName, string avatar)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new BadHttpRequestException("User not found");
            }
            user.Name = newName;
            user.Image = avatar;    
            await _context.SaveChangesAsync();  
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Where(p => p.Email.Trim() == email.Trim()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetListUserFollower(Guid userId)
        {
            return await _context.Follows.Where(p => p.UserToId == userId).Include(p => p.UserFrom).Select(p => p.UserFrom).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetListUserFollowing(Guid userId)
        {
            return await _context.Follows.Where(p => p.UserFromId == userId).Include(p => p.UserTo).Select(p => p.UserTo).ToListAsync();
        }

        public async Task<User> SignIn(string email, string password)
        {
            var user = await _context.Users.Where(p => p.Email == email && p.Password == password).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new BadHttpRequestException("Wrong username or password");
            }
            return user;
        }

        public async Task<User> SignInWithFB(string fbId)
        {
            var user = await _context.Users.Where(p => p.FbId == fbId).FirstOrDefaultAsync();
            if (user == null)
            {
                user = new User() { FbId = fbId, UserId = Guid.NewGuid() };
                _context.Users.Add(user);
            }
            return user;
        }

        public async Task<User> SignUp(string email, string password)
        {
            var user = await _context.Users.Where(p => p.Email == email).FirstOrDefaultAsync();
            if(user != null)
            {
                throw new BadHttpRequestException("This email is already exist");
            }
            user = new User() { UserId = Guid.NewGuid(), Email = email, Password = password };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}

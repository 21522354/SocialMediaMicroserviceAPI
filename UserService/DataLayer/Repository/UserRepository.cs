using Microsoft.EntityFrameworkCore;
using UserService.DataLayer.Models;

namespace UserService.DataLayer.Repository
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        private readonly UserDBContext _context;
        public UserRepository(UserDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Where(p => p.Email.Trim() == email.Trim()).FirstOrDefaultAsync();
        }
    }
}

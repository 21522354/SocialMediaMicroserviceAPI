using Microsoft.AspNetCore.Mvc;
using UserService.DataLayer.DTOs;
using UserService.DataLayer.Models;

namespace UserService.DataLayer.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDBContext _context;

        public UserRepository(UserDBContext context)
        {
            _context = context;
        }
        public void AddUser(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException("User is null");
            }
            _context.Users.Add(user);   
        }

        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null");
            }
            _context.Users.Remove(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }

        public User GetUserById(Guid userId)
        {
            if(userId == null)
            {
                throw new ArgumentNullException("User is null");
            }
            var user = _context.Users.Find(userId);
            if(user == null)
            {
                throw new ArgumentNullException("Not found this user");
            }
            return user;    
        }

        public bool SaveChange()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is null");
            }
            var _user = _context.Users.Find(user.UserId);
            _context.Entry(_user).CurrentValues.SetValues(user);
        }
    }
}

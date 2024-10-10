using Microsoft.AspNetCore.Mvc;
using UserService.DataLayer.DTOs;
using UserService.DataLayer.Models;

namespace UserService.DataLayer.Repository
{
    public interface IUserRepository
    {
        bool SaveChange();
        IEnumerable<User> GetAllUsers();
        User GetUserById(Guid userId);
        void AddUser(User user);        
        void UpdateUser(User user); 
        void DeleteUser(User user);
    }
}

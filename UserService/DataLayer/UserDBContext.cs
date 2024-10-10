using Microsoft.EntityFrameworkCore;
using UserService.DataLayer.Models;

namespace UserService.DataLayer
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }              
        public DbSet<UserFollow> Follows { get; set; }
    }
}

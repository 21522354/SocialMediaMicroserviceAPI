using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using IdentityService.DataLayer.Models;

namespace IdentityService.DataLayer
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }
        public DbSet<Identity> Identitys { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)  
        {

        }
    }
}

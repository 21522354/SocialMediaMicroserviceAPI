﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using UserService.DataLayer.Models;

namespace UserService.DataLayer
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }              
        public DbSet<UserFollow> Follows { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
            .Property(u => u.FbId)
            .IsRequired(false);  // Đảm bảo rằng nó không bắt buộc
            modelBuilder.Entity<User>()
                .Property(u => u.Avatar)
                .IsRequired(false);
            modelBuilder.Entity<User>()
                .Property(u => u.FullName)
                .IsRequired(false);
            modelBuilder.Entity<User>()
                .Property(u => u.NickName)
                .IsRequired(false);

            modelBuilder.Entity<UserFollow>()
                .HasOne(f => f.UserFrom)
                .WithMany()
                .HasForeignKey(f => f.UserFromId)
                .OnDelete(DeleteBehavior.Restrict); // hoặc .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserFollow>()
                .HasOne(f => f.UserTo)
                .WithMany()
                .HasForeignKey(f => f.UserToId)
                .OnDelete(DeleteBehavior.Restrict); // hoặc .OnDelete(DeleteBehavior.NoAction);

        }
    }
}

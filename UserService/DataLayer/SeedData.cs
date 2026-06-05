using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using UserService.DataLayer.Models;

namespace UserService.DataLayer
{
    public static class SeedData
    {
        public static void seedData(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetRequiredService<UserDBContext>();

                //_context.Database.Migrate();

                if(!_context.Users.Any())
                {
                    Console.WriteLine("Seeding data");
                    _context.Users.AddRange(
                        new User()
                        {
                            NickName = "nva_123",
                            FullName = "Nguyen Van A",
                            Email = "ndam8176@gmail.com",
                            Password = "123123",
                            Avatar = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-anh-cute-anime-008.jpg",
                            FbId = "asdadfasdf"
                        },
                        new User()
                        {
                            NickName = "nvb_123",
                            FullName = "Nguyen Van B",
                            Email = "ndam8177@gmail.com",
                            Password = "123123",
                            Avatar = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-anh-cute-anime-018.jpg",
                            FbId = "asdadfasdf"
                        },
                        new User()
                        {
                            FullName = "Nguyen Van C",
                            NickName = "nvc_123",
                            Email = "ndam8178@gmail.com",
                            Password = "123123",
                            Avatar = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-anh-cute-anime-017.jpg",
                            FbId = "asdadfasdf"
                        },
                        new User()
                        {
                            NickName = "nvd_123",
                            FullName = "Nguyen Van D",
                            Email = "ndam8179@gmail.com",
                            Password = "123123",
                            Avatar = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-anh-cute-anime-022.jpg",
                            FbId = "asdadfasdf"
                        },
                        new User()
                        {
                            NickName = "nve_123",
                            FullName = "Nguyen Van E",
                            Email = "ndam8180@gmail.com",
                            Password = "123123",
                            Avatar = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-anh-cute-anime-029.jpg",
                            FbId = "asdadfasdf"
                        });
                    _context.SaveChanges();
                }
                 
            }
        }
    }
}

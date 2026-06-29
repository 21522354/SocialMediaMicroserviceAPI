using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using IdentityService.DataLayer.Models;

namespace IdentityService.DataLayer
{
    public static class SeedData
    {
        public static void seedData(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetRequiredService<IdentityDBContext>();

                //_context.Database.Migrate();

                if(!_context.Identitys.Any())
                {
                    Console.WriteLine("Seeding data");
                    _context.Identitys.AddRange(
                        new Identity()
                        {
                            NickName = "nva_123",
                            FullName = "Nguyen Van A",
                            Email = "ndam8176@gmail.com",
                            Password = "123123",
                            Avatar = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-anh-cute-anime-008.jpg",
                            FbId = "asdadfasdf"
                        },
                        new Identity()
                        {
                            NickName = "nvb_123",
                            FullName = "Nguyen Van B",
                            Email = "ndam8177@gmail.com",
                            Password = "123123",
                            Avatar = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-anh-cute-anime-018.jpg",
                            FbId = "asdadfasdf"
                        },
                        new Identity()
                        {
                            FullName = "Nguyen Van C",
                            NickName = "nvc_123",
                            Email = "ndam8178@gmail.com",
                            Password = "123123",
                            Avatar = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-anh-cute-anime-017.jpg",
                            FbId = "asdadfasdf"
                        },
                        new Identity()
                        {
                            NickName = "nvd_123",
                            FullName = "Nguyen Van D",
                            Email = "ndam8179@gmail.com",
                            Password = "123123",
                            Avatar = "https://www.vietnamworks.com/hrinsider/wp-content/uploads/2023/12/hinh-anh-cute-anime-022.jpg",
                            FbId = "asdadfasdf"
                        },
                        new Identity()
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

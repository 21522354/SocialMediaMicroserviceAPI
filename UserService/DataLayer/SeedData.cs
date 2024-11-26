using UserService.DataLayer.Models;

namespace UserService.DataLayer
{
    public static class SeedData
    {
        public static void seedData(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<UserDBContext>();
                Console.WriteLine("Seeding data");
                _context.Users.AddRange(
                    new User()
                    {
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                        NickName = "nva_123",
                        FullName = "Nguyen Van A",
                        Email = "ndam8175@gmail.com",
                        Password = "123123",
                        Avatar = "abc.png",
                        FbId = "asdadfasdf"
                    },
                    new User()
                    {
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2"),
                        NickName = "nvb_123",
                        FullName = "Nguyen Van B",
                        Email = "ndam8175@gmail.com",
                        Password = "123123",
                        Avatar = "abc.png",
                        FbId = "asdadfasdf"
                    },
                    new User()
                    {
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d3"),
                        FullName = "Nguyen Van C",
                        NickName = "nvc_123",
                        Email = "ndam8175@gmail.com",
                        Password = "123123",
                        Avatar = "abc.png",
                        FbId = "asdadfasdf"
                    },
                    new User()
                    {
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4"),
                        NickName = "nvd_123",
                        FullName = "Nguyen Van D",
                        Email = "ndam8175@gmail.com",
                        Password = "123123",
                        Avatar = "abc.png",
                        FbId = "asdadfasdf"
                    },
                    new User()
                    {
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d5"),
                        NickName = "nve_123",
                        FullName = "Nguyen Van E",
                        Email = "ndam8175@gmail.com",
                        Password = "123123",
                        Avatar = "abc.png",
                        FbId = "asdadfasdf"
                    });
                _context.Follows.AddRange(
                    new UserFollow()
                    {
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2")
                    },
                    new UserFollow()
                    {
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d3")
                    },
                    new UserFollow()
                    {
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4")
                    },
                    new UserFollow()
                    {
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4")
                    },
                    new UserFollow()
                    {
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1")
                    },
                    new UserFollow()
                    {
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d3")
                    },
                    new UserFollow()
                    {
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d3"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4")
                    });
                _context.SaveChanges();     
            }
        }
    }
}

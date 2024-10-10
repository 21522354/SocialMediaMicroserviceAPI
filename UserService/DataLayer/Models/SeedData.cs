namespace UserService.DataLayer.Models
{
    public static class SeedData
    {
        public static void seedData(this IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<UserDBContext>();
                Console.WriteLine("Seeding data");
                _context.Users.AddRange(
                    new User() {
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                        Name = "Nguyen Van A",
                        Email = "ndam8175@gmail.com",
                        Password = "123123",
                        Image = "abc.png",
                        FbId = "asdadfasdf"
                    },
                    new User()
                    {
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2"),
                        Name = "Nguyen Van B",
                        Email = "ndam8175@gmail.com",
                        Password = "123123",
                        Image = "abc.png",
                        FbId = "asdadfasdf"
                    },
                    new User()
                    {
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d3"),
                        Name = "Nguyen Van C",
                        Email = "ndam8175@gmail.com",
                        Password = "123123",
                        Image = "abc.png",
                        FbId = "asdadfasdf"
                    },
                    new User()
                    {
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4"),
                        Name = "Nguyen Van D",
                        Email = "ndam8175@gmail.com",
                        Password = "123123",
                        Image = "abc.png",
                        FbId = "asdadfasdf"
                    },
                    new User()
                    {
                        UserId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d5"),
                        Name = "Nguyen Van E",
                        Email = "ndam8175@gmail.com",
                        Password = "123123",
                        Image = "abc.png",
                        FbId = "asdadfasdf"
                    });
                _context.Follows.AddRange(
                    new UserFollow()
                    {
                        Id = 1,
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2")
                    },
                    new UserFollow()
                    {
                        Id = 1,
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d3")
                    },
                    new UserFollow()
                    {
                        Id = 1,
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4")
                    },
                    new UserFollow()
                    {
                        Id = 1,
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4")
                    },
                    new UserFollow()
                    {
                        Id = 1,
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1")
                    },
                    new UserFollow()
                    {
                        Id = 1,
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2")
                    },
                    new UserFollow()
                    {
                        Id = 1,
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d3")
                    },
                    new UserFollow()
                    {
                        Id = 1,
                        UserFromId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d3"),
                        UserToId = Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4")
                    });

            }
        }
    }
}

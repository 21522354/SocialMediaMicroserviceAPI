namespace PostService.Data_Layer
{
    public static class SeedData
    {
        public static void seedData(this IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<PostServiceDBContext>();
                Console.WriteLine("Seeding data");
            }
        }
    }
}


using NotificationService.DataLayer;
using NotificationService.Hubs;

namespace NotificationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApplicationService(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.WithOrigins("http://localhost:3000") // Thay bằng origin của frontend
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // Quan trọng với SignalR
                });
            });
            builder.Services.AddSignalR();

            Console.WriteLine($"{builder.Configuration["RabbitMQHost"]}/{builder.Configuration["RabbitMQPort"]}");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowSpecificOrigins");

            app.MapControllers();

            app.MigrateDatabase();

            app.MapHub<NotificationHub>("/notificationHub");

            app.Run();
        }
    }
}

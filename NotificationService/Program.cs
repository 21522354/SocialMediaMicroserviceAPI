
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
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("http://localhost:3000",
                        "http://localhost:5216",
                        "http://192.168.1.1",
                        "http://192.168.1.2",
                        "http://192.168.1.3",
                        "http://192.168.1.4",
                        "http://192.168.1.5",
                        "http://192.168.1.6",
                        "http://192.168.1.7",
                        "http://192.168.1.8",
                        "http://192.168.1.9",
                        "http://192.168.1.10"
                        ) // Thêm các origin cụ thể
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // Cho phép gửi thông tin đăng nhập
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

            app.UseCors("AllowAll");

            app.MapControllers();

            app.MigrateDatabase();

            app.MapHub<NotificationHub>("/notificationHub").RequireCors("AllowAll");

            app.Run();
        }
    }
}

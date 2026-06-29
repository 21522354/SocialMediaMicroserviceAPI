using Microsoft.EntityFrameworkCore;
using IdentityService;
using IdentityService.DataLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Thêm CORS
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

var app = builder.Build();

if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Sử dụng CORS
app.UseCors("AllowSpecificOrigins");

app.UseExceptionHandler("/error");

app.MapControllers();

app.seedData();

app.Run();

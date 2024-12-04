using UserService;
using UserService.DataLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationService(builder.Configuration);

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

using ChessBackende8.Services; // Include this namespace if needed for custom services
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; // Add this namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DbContext with the connection string from configuration
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register any custom services for dependency injection
builder.Services.AddScoped<GamesService>();
builder.Services.AddScoped<Logic>();
builder.Services.AddHttpContextAccessor(); // Add this line if not already there

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware for logging errors
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        // Log the exception
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An unhandled exception occurred while processing the request.");

        // Return a 500 Internal Server Error response
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An internal server error occurred.");
    }
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Call SeedUsers directly at startup
SeedUsers();
app.Run();

void SeedUsers()
{
    // Hardcoded user details
    var users = new List<(string UserName, string Email, string Password)>
    {
        ("Test", "Test", "Geheim"),
        ("Guest", "Guest", "Password")
    };

    foreach (var user in users)
    {
        Console.WriteLine($"Seeded User: Username = {user.UserName}, Email = {user.Email}");
    }
}

using ChessBackende8.Services; // Include this namespace if needed for custom services
using Microsoft.EntityFrameworkCore;

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
// Add this line in Program.cs if not already there
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Call SeedUsers directly at startup
SeedUsers();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
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



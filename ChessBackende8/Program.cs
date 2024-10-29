using ChessBackende8.Services; // Include this namespace if needed for custom services
using Microsoft.EntityFrameworkCore;
using ChessBackende8.Controllers.Entities; // Add this namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DbContext with in-memory database
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseInMemoryDatabase("ChessGamesDb"); // Specify a name for the in-memory database
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

// Call SeedData directly at startup (optional, only if you want to seed the database)
SeedData(app.Services);
app.Run();

void SeedData(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        // Optionally seed the in-memory database with initial data
        var games = new List<GameMe>
        {
            new GameMe { positions = "Sample PGN 1", isWhite = true, whiteWon = false },
            new GameMe { positions = "Sample PGN 2", isWhite = false, whiteWon = true }
        };

        context.Games.AddRange(games);
        context.SaveChanges(); // Save changes to the in-memory database

        Console.WriteLine("Seeded initial game data.");
    }
}

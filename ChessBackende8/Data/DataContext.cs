using Microsoft.EntityFrameworkCore;
using ChessBackende8.Controllers.Entities;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<GameMe> Games { get; set; } 
}


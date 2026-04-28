using Microsoft.EntityFrameworkCore;

namespace AuthDemoApi;

// 1. The Database Model (Entity)
// This represents how data is structured in SQLite database
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; 
}

// 2. Data Transfer Object (DTO)
// Used to securely receive data from the frontend without exposing the internal DB model
public class AuthDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// 3. The Database Context
// This is the bridge between C# code and the SQLite database
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    // Creates a "Users" table in the database
    public DbSet<User> Users { get; set; }
}
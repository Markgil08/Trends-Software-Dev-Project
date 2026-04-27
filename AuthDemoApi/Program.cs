using AuthDemoApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURE SERVICES ---

builder.Services.AddControllers();

// Configure SQLite Database
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite("Data Source=authdemo.db"));

// Configure CORS (Crucial for the Frontend to connect)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Force the app to run on port 5264 to match the HTML file
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5264);
});

var app = builder.Build();

// --- 2. CONFIGURE PIPELINE ---

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
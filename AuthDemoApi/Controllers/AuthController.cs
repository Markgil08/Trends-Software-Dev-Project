using Microsoft.AspNetCore.Mvc;

namespace AuthDemoApi.Controllers;

[ApiController]
[Route("api/auth")] // Base route for all actions in this controller
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public IActionResult Register([FromBody] AuthDto request)
    {
        if (_db.Users.Any(u => u.Username == request.Username))
        {
            return BadRequest(new { message = "Username already exists." });
        }

        var newUser = new User 
        { 
            Username = request.Username, 
            Password = request.Password 
        };
        
        _db.Users.Add(newUser);
        _db.SaveChanges();

        return Ok(new { message = "Registration successful!" });
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public IActionResult Login([FromBody] AuthDto request)
    {
        var user = _db.Users.FirstOrDefault(u => 
            u.Username == request.Username && 
            u.Password == request.Password);
        
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        return Ok(new { message = "Login successful!", username = user.Username });
    }

    // DELETE: api/auth/delete/{username}
    // This matches the fetch(`${API_BASE_URL}/delete/${currentUser}`) call
    [HttpDelete("delete/{username}")]
    public IActionResult DeleteAccount(string username)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);
        
        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        _db.Users.Remove(user);
        _db.SaveChanges();

        return Ok(new { message = "Account successfully deleted." });
    }

    // GET: api/auth/test
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new { message = "API is online and reachable!" });
    }
}
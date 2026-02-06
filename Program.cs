using Microsoft.EntityFrameworkCore;
using Azure.Monitor.OpenTelemetry.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- SERVICES ---
// Final Version with Login
builder.Services.AddDbContext<TechUnityDbContext>(options =>
    options.UseInMemoryDatabase("TechUnityDb"));

var app = builder.Build();
app.UseStaticFiles();

// --- 1. HOME & LOGIN PAGE ---
app.MapGet("/", () => Results.Content(@$"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>TechUnity | Login & Register</title>
    <style>
        body {{ font-family: sans-serif; background: #0a0a0f; color: white; text-align: center; padding-top: 50px; }}
        .auth-box {{ background: #1a1a24; padding: 30px; border-radius: 15px; display: inline-block; border: 1px solid #f39c12; width: 350px; }}
        input {{ width: 90%; padding: 10px; margin: 10px 0; border-radius: 5px; border: none; }}
        button {{ background: #f39c12; color: #0a0a0f; padding: 10px 20px; border: none; border-radius: 5px; cursor: pointer; font-weight: bold; width: 95%; }}
        .msg {{ color: #f39c12; margin-top: 15px; font-weight: bold; }}
    </style>
</head>
<body>
    <h1>TechUnity Portal</h1>
    
    <div class=""auth-box"">
        <h2 id=""form-title"">Login</h2>
        <form id=""authForm"">
            <input type=""text"" id=""username"" placeholder=""Username"" required>
            <input type=""password"" id=""password"" placeholder=""Password"" required>
            <button type=""submit"" id=""submitBtn"">Enter Platform</button>
        </form>
        <p class=""msg"" id=""message""></p>
        <p style=""font-size: 0.8rem; cursor: pointer; text-decoration: underline;"" onclick=""toggleForm()"">Switch Login/Register</p>
    </div>

    <script>
        let isLogin = true;
        function toggleForm() {{
            isLogin = !isLogin;
            document.getElementById('form-title').innerText = isLogin ? 'Login' : 'Register';
            document.getElementById('submitBtn').innerText = isLogin ? 'Enter Platform' : 'Create Account';
        }}

        document.getElementById('authForm').onsubmit = async (e) => {{
            e.preventDefault();
            const username = document.getElementById('username').value;
            const password = document.getElementById('password').value;
            const endpoint = isLogin ? '/login' : '/register';

            const response = await fetch(endpoint, {{
                method: 'POST',
                headers: {{ 'Content-Type': 'application/json' }},
                body: JSON.stringify({{ username, password }})
            }});

            const result = await response.text();
            document.getElementById('message').innerText = result;
            if(result.includes('Success')) {{
                 setTimeout(() => alert('Welcome to TechUnity Community!'), 500);
            }}
        }};
    </script>
</body>
</html>
", "text/html"));

// --- 2. AUTHENTICATION LOGIC ---

// Registration Endpoint
app.MapPost("/register", async (User newUser, TechUnityDbContext db) => {
    if (await db.Users.AnyAsync(u => u.Username == newUser.Username))
        return Results.BadRequest("User already exists.");
    
    db.Users.Add(newUser);
    await db.SaveChangesAsync();
    return Results.Ok("Registration Successful! Now please login.");
});

// Login Endpoint (Your Specific Credentials Check)
app.MapPost("/login", async (User loginAttempt, TechUnityDbContext db) => {
    // Check your specific credentials first
    if (loginAttempt.Username == "oluwafaj" && loginAttempt.Password == "Aeroaj16") {
        return Results.Ok("Success: Welcome back, FAJAdministrator.");
    }

    // Check database for registered users
    var user = await db.Users.FirstOrDefaultAsync(u => 
        u.Username == loginAttempt.Username && u.Password == loginAttempt.Password);
    
    if (user != null) return Results.Ok("Login Success!");
    
    return Results.BadRequest("Invalid Username or Password.");
});

app.Run();

// --- 3. MODELS (BOTTOM) ---
public record User(string Username, string Password);

public class TechUnityDbContext : DbContext {
    public TechUnityDbContext(DbContextOptions<TechUnityDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<Mentor> Mentors => Set<Mentor>();
}

public record Mentor(Guid Id, string Name, string Expertise, string Location, bool IsActive);
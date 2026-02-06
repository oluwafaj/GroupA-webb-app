using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TechUnityDbContext>(options =>
    options.UseInMemoryDatabase("TechUnityDb"));

var app = builder.Build();
app.UseStaticFiles(); // This enables the /images folder

app.MapGet("/", () => Results.Content(@$"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>TechUnity Portal</title>
    <style>
        body {{ font-family: 'Segoe UI', sans-serif; background: #0a0a0f; color: white; text-align: center; padding: 50px; }}
        .auth-box {{ background: #1a1a24; padding: 30px; border-radius: 15px; display: inline-block; border: 1px solid #f39c12; width: 350px; }}
        .dashboard {{ display: none; background: #1a1a24; padding: 40px; border-radius: 15px; border: 2px solid #3498db; }}
        input {{ width: 90%; padding: 12px; margin: 10px 0; border-radius: 5px; border: 1px solid #333; background: #0a0a0f; color: white; }}
        button {{ background: #f39c12; color: #0a0a0f; padding: 12px; border: none; border-radius: 5px; cursor: pointer; font-weight: bold; width: 97%; }}
        .hero-img {{ width: 100%; max-width: 500px; border-radius: 15px; border: 2px solid #f39c12; margin-bottom: 20px; }}
    </style>
</head>
<body>
    <img src=""/images/students.jpg"" alt=""Image not found in wwwroot/images/"" class=""hero-img"">

    <div id=""login-section"" class=""auth-box"">
        <h2>TechUnity Login</h2>
        <form id=""authForm"">
            <input type=""text"" id=""username"" placeholder=""Username"" required>
            <input type=""password"" id=""password"" placeholder=""Password"" required>
            <button type=""submit"">Enter Platform</button>
        </form>
        <div id=""message"" style=""margin-top:15px; color:#f39c12;""></div>
    </div>

    <div id=""dashboard-section"" class=""dashboard"">
        <h1 style=""color:#3498db"">Welcome, FAJ Admin</h1>
        <p>System Status: <span style=""color:#2ecc71"">Online</span></p>
        <div style=""display: flex; justify-content: space-around; margin-top: 20px;"">
            <div style=""padding:20px; border:1px solid #333;"">Total Students<br><strong>1,240</strong></div>
            <div style=""padding:20px; border:1px solid #333;"">Active Mentors<br><strong>85</strong></div>
        </div>
        <button onclick=""location.reload()"" style=""background:#e74c3c; width:auto; margin-top:20px;"">Logout</button>
    </div>

    <script>
        document.getElementById('authForm').onsubmit = async (e) => {{
            e.preventDefault();
            const user = document.getElementById('username').value;
            const pass = document.getElementById('password').value;

            const response = await fetch('/login', {{
                method: 'POST',
                headers: {{ 'Content-Type': 'application/json' }},
                body: JSON.stringify({{ Username: user, Password: pass }})
            }});

            const result = await response.text();
            if (response.ok && result.includes('FAJAdministrator')) {{
                document.getElementById('login-section').style.display = 'none';
                document.getElementById('dashboard-section').style.display = 'block';
            }} else {{
                document.getElementById('message').innerText = result;
            }}
        }};
    </script>
</body>
</html>
", "text/html"));

// Keep the same /login and /register endpoints from before
app.MapPost("/login", async ([FromBody] User loginAttempt, TechUnityDbContext db) => {
    if (loginAttempt.Username == "oluwafaj" && loginAttempt.Password == "Aeroaj16") {
        return Results.Ok("Success: Welcome back, FAJAdministrator.");
    }
    return Results.BadRequest("Invalid Credentials.");
});

app.Run();

public record User(string Username, string Password);
public class TechUnityDbContext : DbContext {
    public TechUnityDbContext(DbContextOptions<TechUnityDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
}
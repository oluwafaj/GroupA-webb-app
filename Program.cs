using Microsoft.EntityFrameworkCore;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// --- MONITORING & DATABASE ---
var aiConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
if (!string.IsNullOrEmpty(aiConnectionString))
{
    builder.Services.AddOpenTelemetry().UseAzureMonitor();
}

builder.Services.AddDbContext<TechUnityDbContext>(options =>
    options.UseInMemoryDatabase("TechUnityDb"));

var app = builder.Build();

// Enable serving images from wwwroot/images
app.UseStaticFiles(); 

// --- 1. LOGIN PAGE (With Image & Password) ---
app.MapGet("/", () => Results.Content(@$"
    <!DOCTYPE html>
    <html>
    <head>
        <title>TechUnity | Login</title>
        <style>
            body {{ font-family: 'Segoe UI', sans-serif; background-color: #121212; color: white; text-align: center; padding-top: 50px; }}
            .login-card {{ background: #1e1e1e; padding: 40px; border-radius: 15px; display: inline-block; box-shadow: 0 10px 30px rgba(0,0,0,0.5); }}
            .profile-img {{ width: 250px; height: auto; border-radius: 10px; border: 3px solid #f39c12; margin-bottom: 20px; }}
            input {{ display: block; width: 280px; margin: 10px auto; padding: 12px; border-radius: 5px; border: none; }}
            button {{ background: #f39c12; color: black; font-weight: bold; border: none; padding: 12px 30px; border-radius: 5px; cursor: pointer; transition: 0.3s; }}
            button:hover {{ background: #e67e22; }}
            a {{ color: #3498db; text-decoration: none; font-size: 0.9em; }}
        </style>
    </head>
    <body>
        <div class='login-card'>
            <img src='/images/students.jpg' class='profile-img' alt='Black African Students in Tech' />
            <h1>TechUnity</h1>
            <p>Empowering African Tech Excellence</p>
            <input type='text' placeholder='Username' />
            <input type='password' placeholder='Password' />
            <button onclick=""alert('Welcome to the Network!')"">Login</button>
            <br><br>
            <a href='/contact'>Need help? Contact Us</a>
        </div>
    </body>
    </html>
", "text/html"));

// --- 2. CONTACT US PAGE ---
app.MapGet("/contact", () => Results.Content(@"
    <html>
    <body style='font-family: sans-serif; text-align: center; padding: 50px;'>
        <h1>Contact Us</h1>
        <p>Email: <b>connect@techunity.africa</b></p>
        <p>Join our community hubs in Lagos, Nairobi, and Accra.</p>
        <a href='/'>Back to Login</a>
    </body>
    </html>
", "text/html"));

// --- 3. API ENDPOINTS (For Mentors) ---
app.MapGet("/api/mentors", async (TechUnityDbContext db) => await db.Mentors.ToListAsync());

// Seed some data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TechUnityDbContext>();
    if (!db.Mentors.Any())
    {
        db.Mentors.Add(new Mentor(Guid.NewGuid(), "Aisha Koné", "Cloud Architecture", "Abidjan", true));
        db.SaveChanges();
    }
}

app.Run();

// --- TYPES & CONTEXT ---
public record Mentor(Guid Id, string Name, string Expertise, string Location, bool IsActive);
public class TechUnityDbContext : DbContext {
    public TechUnityDbContext(DbContextOptions<TechUnityDbContext> options) : base(options) { }
    public DbSet<Mentor> Mentors => Set<Mentor>();
}
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
    <title>TechUnity | African Excellence in Tech</title>
    <style>
        :root {{ --primary: #f39c12; --secondary: #3498db; --dark: #0a0a0f; --card: #1a1a24; }}
        body {{ font-family: 'Segoe UI', sans-serif; background: var(--dark); color: white; margin: 0; padding: 0; }}
        
        /* Navigation Bar */
        nav {{ background: #111; padding: 1rem; display: flex; justify-content: center; gap: 30px; border-bottom: 1px solid #333; position: sticky; top: 0; z-index: 100; }}
        nav a {{ color: white; text-decoration: none; font-weight: bold; cursor: pointer; transition: 0.3s; }}
        nav a:hover {{ color: var(--primary); }}

        .container {{ max-width: 900px; margin: auto; padding: 40px 20px; }}
        .section {{ display: none; animation: fadeIn 0.5s; }}
        .active {{ display: block; }}
        
        @keyframes fadeIn {{ from {{ opacity: 0; }} to {{ opacity: 1; }} }}

        .hero-img {{ width: 100%; max-width: 600px; border-radius: 15px; border: 2px solid var(--primary); margin-bottom: 20px; }}
        
        /* Hub Cards Style */
        .hubs-grid {{ display: grid; grid-template-columns: repeat(auto-fit, minmax(140px, 1fr)); gap: 15px; margin-top: 20px; }}
        .hub-card {{ background: var(--card); padding: 15px; border-radius: 10px; border: 1px solid #333; text-align: center; }}
    </style>
</head>
<body>

    <nav>
        <a onclick=""showSection('home')"">Home</a>
        <a onclick=""showSection('about')"">About Us</a>
        <a onclick=""showSection('contact')"">Contact</a>
    </nav>

    <div class=""container"">
        <div id=""home"" class=""section active"">
            <img src=""/images/students.jpg"" alt=""Black African Students in Tech"" class=""hero-img"">
            <h1 style=""color: var(--primary);"">Empowering Black African Students</h1>
            <p style=""font-size: 1.2rem; line-height: 1.6;"">
                Welcome to TechUnity. We are a premier global community dedicated to bridging the gap for African talent in the digital age.
            </p>
            <button onclick=""showSection('about')"" style=""background: var(--primary); padding: 10px 20px; border: none; border-radius: 5px; cursor: pointer; font-weight: bold;"">Explore Our Mission</button>
        </div>

        <div id=""about"" class=""section"">
            <h2 style=""color: var(--secondary);"">Our Mission</h2>
            <p style=""text-align: left; line-height: 1.8;"">
                TechUnity serves as a bridge, bringing <b>Black African students</b> together through technology. 
                We believe that by providing access to Azure cloud resources, expert mentorship, and a global network, 
                we can foster the next generation of African tech leaders thank you for the encouragement Bethel.
            </p>
            
            <h3>Our Global Community Hubs</h3>
            <div class=""hubs-grid"">
                <div class=""hub-card"">🇳🇬<br>Lagos</div>
                <div class=""hub-card"">🇰🇪<br>Nairobi</div>
                <div class=""hub-card"">🇿🇦<br>Cape Town</div>
                <div class=""hub-card"">🇬🇭<br>Accra</div>
            </div>
        </div>

        <div id=""contact"" class=""section"">
            <h2 style=""color: var(--primary);"">Get In Touch</h2>
            <p>Ready to join the community or need technical support?</p>
            <div style=""background: var(--card); padding: 20px; border-radius: 10px;"">
                <p>📧 Email: <b>connect@techunity.africa</b></p>
                <p>📍 Location: Global Virtual Community</p>
                <p>Support Hours: 24/7 via Community Discord</p>
            </div>
        </div>
    </div>

    <script>
        function showSection(id) {{
            document.querySelectorAll('.section').forEach(s => s.classList.remove('active'));
            document.getElementById(id).classList.add('active');
        }}
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
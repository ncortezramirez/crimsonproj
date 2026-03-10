using mis_221_pa_5_ncortezramirez_1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add CORS to allow HTML frontend to call API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Serve static files (your index.html)
app.UseDefaultFiles();
app.UseStaticFiles();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
Console.WriteLine("===========================================");
Console.WriteLine("🚀 Crimson Sports API is running!");
Console.WriteLine("===========================================");
Console.WriteLine($"📍 API Endpoints: http://0.0.0.0:{port}/api");
Console.WriteLine($"🌐 Web UI: http://0.0.0.0:{port}/index.html");
Console.WriteLine("===========================================");

app.Run($"http://0.0.0.0:{port}");
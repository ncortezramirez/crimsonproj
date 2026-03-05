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
app.UseStaticFiles();
app.UseDefaultFiles();

Console.WriteLine("===========================================");
Console.WriteLine("🚀 Crimson Sports API is running!");
Console.WriteLine("===========================================");
Console.WriteLine("📍 API Endpoints: http://localhost:5000/api");
Console.WriteLine("🌐 Web UI: http://localhost:5000/index.html");
Console.WriteLine("===========================================");

app.Run("http://localhost:5000");
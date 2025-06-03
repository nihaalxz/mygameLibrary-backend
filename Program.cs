using apiWebpractice.VideoGameDBContext;
using Microsoft.EntityFrameworkCore;

namespace apiWebpractice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();

            // ? Commented out for debugging formatting error
            // builder.Services.AddOpenApi();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Production", policy =>
                {
                    policy.WithOrigins("https://gameslib-ggdud6h8fychh0dj.uaenorth-01.azurewebsites.net")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Enable middleware
            if (app.Environment.IsDevelopment())
            {
                // ? Commented out for debugging formatting error
                // app.MapScalarApiReference();
                // app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            // Apply CORS
            app.UseCors("Production");

            app.UseAuthorization();

            // Serve static files (Angular build goes into wwwroot)
            app.UseDefaultFiles(); // Looks for index.html
            app.UseStaticFiles();

            // Map API controllers
            app.MapControllers();

            // Fallback to index.html for Angular routing
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}

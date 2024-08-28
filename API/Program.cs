using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<StoreContext>();

                // Attempt to run a query to check the connection
                try
                {
                    // This is a basic query that checks if the connection works
                    var canConnect = context.Database.CanConnect();
                    if (canConnect)
                    {
                        Console.WriteLine("Successfully connected to the database.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to connect to the database.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while trying to connect to the database: {ex.Message}");
                }
            }

            app.MapControllers();

            app.Run();
        }
    }
}
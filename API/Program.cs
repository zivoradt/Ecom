using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    var service = scope.ServiceProvider;
                    var context = service.GetRequiredService<StoreContext>();
                    var logger = service.GetRequiredService<ILogger<StoreContextSeed>>();

                    await StoreContextSeed.CheckDatabaseConnection(context, logger);
                    await context.Database.MigrateAsync();
                    await StoreContextSeed.SeedAsync(context, logger);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            app.MapControllers();

            app.Run();
        }
    }
}
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IncidentApi.Models;
using Microsoft.Extensions.Configuration;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // supprimer ancien DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<IncidentsDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // récupérer connection string depuis CI ou appsettings
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();

            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var connectionString = configuration.GetConnectionString("IncidentsConnection");

            // ajouter SQL Server (Docker CI ou local)
            services.AddDbContext<IncidentsDbContext>(options =>
                options.UseSqlServer(connectionString));

            // rebuild provider
            var serviceProvider = services.BuildServiceProvider();

            using var scope2 = serviceProvider.CreateScope();
            var db = scope2.ServiceProvider.GetRequiredService<IncidentsDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }
}
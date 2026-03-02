using Microsoft.EntityFrameworkCore;
using IncidentApi.Models;

namespace IncidentApi.Modelsss
{
    public class IncidentsDbContext : DbContext
    {
        public IncidentsDbContext(DbContextOptions<IncidentsDbContext> options)
            : base(options)
        {
        }
        public DbSet<IncidentApi.Models.Incident> Incident { get; set; } = default!;

        // Exemple :
        // public DbSet<Incident> Incidents { get; set; }
    }
}
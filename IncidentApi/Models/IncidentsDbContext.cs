using Microsoft.EntityFrameworkCore;
using IncidentApi.Models;

namespace IncidentApi.Models
{
    public class IncidentsDbContext : DbContext
    {
        public IncidentsDbContext(DbContextOptions<IncidentsDbContext> options)
            : base(options)
        {
        }
        public DbSet<IncidentApi.Models.Incident> Incidents { get; set; } = default!;

       

    }
}
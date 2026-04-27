using IncidentApi.Controllers;
using IncidentApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppTests
{
    public class IncidentsTests
    {
        private IncidentsDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<IncidentsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new IncidentsDbContext(options);
        }

        [Theory]
        [InlineData("LOW")]
        [InlineData("MEDIUM")]
        [InlineData("HIGH")]
        [InlineData("CRITICAL")]
        public async Task PostIncident_ValidSeverity_ReturnsCreated(string severity)
        {
            var context = GetDbContext();
            var controller = new IncidentsController(context);

            var incident = new Incident
            {
                Title = "Test",
                Description = "Test description",
                Severity = severity,
                Status = "OPEN"
            };

            var result = await controller.PostIncident(incident);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdIncident = Assert.IsType<Incident>(createdResult.Value);

            Assert.Equal(severity, createdIncident.Severity);
            Assert.Equal("OPEN", createdIncident.Status);
            Assert.NotNull(createdIncident.CreatedAt);
        }
    }
}
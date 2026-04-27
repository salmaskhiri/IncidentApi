using IncidentApi.Models;
using System.Net.Http.Json;

namespace AppTests
{
    public class IncidentsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public IncidentsIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetIncidents_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/incidents/get-all");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PostIncident_CreatesIncident()
        {
            var incident = new
            {
                Title = "Test Incident",
                Description = "Test Description",
                Severity = "HIGH"
            };

            var response = await _client.PostAsJsonAsync("/api/incidents", incident);
            response.EnsureSuccessStatusCode();

            var createdIncident = await response.Content.ReadFromJsonAsync<Incident>();

            Assert.NotNull(createdIncident);
            Assert.Equal("Test Incident", createdIncident!.Title);
            Assert.Equal("HIGH", createdIncident.Severity);
        }

        [Fact]
        public async Task PostThenGet_ReturnsInsertedIncident()
        {
            var incident = new
            {
                Title = "Integration Test",
                Description = "Test Description",
                Severity = "MEDIUM"
            };

            // 1. CREATE
            var postResponse = await _client.PostAsJsonAsync("/api/incidents", incident);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<Incident>();

            Assert.NotNull(created);

            // 2. GET ALL (bonne route)
            var response = await _client.GetAsync("/api/incidents/get-all");
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<List<Incident>>();

            Assert.Contains(data!, i => i.Title == "Integration Test");
        }
    }
}
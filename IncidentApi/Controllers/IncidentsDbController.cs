using IncidentApi.Models;
using IncidentApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IncidentAPI_X.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentsController : ControllerBase
    {
        private readonly IncidentsDbContext _context;

        private static readonly string[] AllowedSeverities = { "LOW", "MEDIUM", "HIGH", "CRITICAL" };
        private static readonly string[] AllowedStatuses = { "OPEN", "IN_PROGRESS", "RESOLVED" };

        // ✅ CONSTRUCTOR (fixes your first error)
        public IncidentsController(IncidentsDbContext context)
        {
            _context = context;
        }

        // ✅ GET ALL INCIDENTS
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Incident>>> GetIncidents()
        {
            return await _context.Incidents.ToListAsync();
        }

        // ✅ GET INCIDENT BY ID
        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<Incident>> GetIncidentById(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
                return NotFound();

            return incident;
        }

        // ✅ CREATE INCIDENT
        [HttpPost]
        public async Task<ActionResult<Incident>> PostIncident(Incident incident)
        {
            if (incident == null)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(incident.Severity) ||
                !AllowedSeverities.Contains(incident.Severity.ToUpper()))
            {
                return BadRequest("Invalid severity.");
            }

            incident.Severity = incident.Severity.ToUpper();
            incident.Status = "OPEN";
            incident.CreatedAt = DateTime.Now;

            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            // ✅ IMPORTANT for your test
            return CreatedAtAction(nameof(GetIncidentById), new { id = incident.Id }, incident);
        }

        // ✅ UPDATE INCIDENT (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncident(int id, Incident incident)
        {
            if (id != incident.Id)
                return BadRequest();

            var existingIncident = await _context.Incidents.FindAsync(id);

            if (existingIncident == null)
                return NotFound();

            existingIncident.Title = incident.Title;
            existingIncident.Description = incident.Description;
            existingIncident.Status = incident.Status;
            existingIncident.Severity = incident.Severity;

            _context.Entry(existingIncident).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent(); // ✅ required by your test
        }

        // ✅ DELETE INCIDENT
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
                return NotFound();

            if (incident.Severity == "CRITICAL" && incident.Status == "OPEN")
                return BadRequest("Cannot delete an OPEN CRITICAL incident.");

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ FILTER BY STATUS
        [HttpGet("filter-by-status/{status}")]
        public async Task<IActionResult> FilterByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Status is required.");

            var filtered = await _context.Incidents
                .Where(i => i.Status.ToUpper() == status.ToUpper())
                .ToListAsync();

            return Ok(filtered);
        }

        // ✅ FILTER BY SEVERITY
        [HttpGet("filter-by-severity/{severity}")]
        public async Task<IActionResult> FilterBySeverity(string severity)
        {
            if (string.IsNullOrWhiteSpace(severity))
                return BadRequest("Severity is required.");

            var filtered = await _context.Incidents
                .Where(i => i.Severity.ToUpper() == severity.ToUpper())
                .ToListAsync();

            return Ok(filtered);
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.Id == id);
        }
    }
}
using IncidentApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IncidentApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private static readonly List<Incident> _incidents = new();
        private static int _nextId = 1;
        private static readonly string[] AllowedSeverities =
        { "LOW", "MEDIUM", "HIGH", "CRITICAL" };
        private static readonly string[] AllowedStatuses =
        { "OPEN", "IN_PROGRESS", "RESOLVED" };

        //post api/incidents/create-incident
        [HttpPost("create-incident")]
        public IActionResult CreateIncident([FromBody] Incident incident)
        { if (!AllowedSeverities.Contains(incident.Severity.ToUpper()))
                return BadRequest("Invalid severity value.");
        
            incident.Id = _nextId++;
           
            incident.Status = "OPEN";

       
            _incidents.Add(incident);

           
            return Ok(incident);
        }
        [HttpGet("get-all")]
        public IActionResult GetAllIncidents()
        {
            return Ok(_incidents);
        }
        [HttpGet("getbyid/{id}")]
        public IActionResult GetIncidentById(int id)
        { Incident incident;
            try
            {
                incident = _incidents.First(i => i.Id == id);
            }
                catch(InvalidOperationException)
            {
                return NotFound();
            }
            
               
            return Ok(incident);
        }
        [HttpPut("update-status/{id}")]
        public IActionResult UpdateIncidentStatus(int id, [FromBody] string status)
        {
            
            var incident = _incidents.FirstOrDefault(i => i.Id == id);
            if (incident == null)
                return NotFound();

            incident.Status = status.ToUpper();
            return Ok(incident);
        }
        [HttpDelete("delete-incident/{id}")]
        public IActionResult DeleteIncident(int id)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);
            if (incident == null)
                return NotFound();
            if (incident.Severity == "CRITICAL" && incident.Status == "OPEN")
            {
                return BadRequest("Un incident CRITICAL ne peut pas être supprimé tant qu'il est OPEN.");
            }

            _incidents.Remove(incident);
            return NoContent();
        }
        [HttpGet("filter-by-status")]
        public IActionResult FilterByStatus([FromQuery] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Le paramètre 'status' est requis.");

            var filtered = _incidents.Where(i => i.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(filtered);
        }
        [HttpGet("filter-by-severity")]
        public IActionResult FilterBySeverity([FromQuery] string severity)
        {
            if (string.IsNullOrWhiteSpace(severity))
                return BadRequest("Le paramètre 'status' est requis.");

            var filtered = _incidents.Where(i => i.Status.Equals(severity, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(filtered);
        }
    }
}

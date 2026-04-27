using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncidentApi.Models
{
    [Table("Incidents")]
    public class Incident
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 30 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [RegularExpression("LOW|MEDIUM|HIGH|CRITICAL", ErrorMessage = "Invalid severity")]
        public string Severity { get; set; } = string.Empty;

        public string Status { get; set; } = "OPEN";

        public DateTime CreatedAt { get; set; }
    }
}
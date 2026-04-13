using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // ← à ajouter

namespace IncidentApi.Models
{
    [Table("Incidents")] // <-- correspond au nom exact de la table SQL
    public class Incident
    {
        public int Id { get; set; }
        [RegularExpression("LOW|MEDIUM|HIGH|CRITICAL", ErrorMessage = "Invalid severity")]
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "erreur")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Severity { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
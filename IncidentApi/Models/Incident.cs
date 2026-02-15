using System.Data;
using System.ComponentModel.DataAnnotations;
namespace IncidentApi.Models

{
    public class Incident
    {
        public int Id { get; set; } 
        [Required]
        [StringLength(30, MinimumLength = 3,ErrorMessage="erreur")]
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

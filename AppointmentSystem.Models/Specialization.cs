using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Models
{
   public class Specialization
    {
        [Key]
        public int SpecializationId { get; set; }
        [Required]
        public required string SpecializationName { get; set; }
    }
}

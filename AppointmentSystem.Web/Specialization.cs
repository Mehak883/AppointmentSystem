using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AppointmentSystem.Models
{
    public class Specialization
    {
        [Key]
        public int SpecializationId { get; set; }

        [Required]
        public required string SpecializationName { get; set; }

        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; } = new List<DoctorSpecialization>();
    }
}

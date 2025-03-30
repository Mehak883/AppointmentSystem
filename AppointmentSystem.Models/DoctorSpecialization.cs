using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Models
{
    public class DoctorSpecialization
    {
        [Key]
        public int DoctorSpecializationId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int SpecializationId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } = null!;  

        [ForeignKey("SpecializationId")]
        public Specialization Specialization { get; set; } = null!; 
    }
}


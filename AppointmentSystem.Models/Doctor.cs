using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentSystem.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        public string UserId { get; set; }  

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }  


        [Required]
        [DataType(DataType.Time)]  
        public TimeSpan StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]  
        public TimeSpan EndTime { get; set; }


        [Required]
        [Range(15, 120, ErrorMessage = "Slot duration must be between 15 and 120 minutes.")]
        public int SlotDuration { get; set; }

        public List<DoctorSpecialization> DoctorSpecializations { get; set; } = new();

    }
}

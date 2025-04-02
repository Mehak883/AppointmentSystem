using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentSystem.Models
{
    public class Slot
    {
        [Key]
        public int SlotId { get; set; }  

        [Required]
        public int DoctorId { get; set; }  

        [ForeignKey("DoctorId")]
        public Doctor? Doctor { get; set; }  

        [Required]
        public DateTime Date { get; set; }  

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required]
        public SlotStatus Status { get; set; } = SlotStatus.Available;  
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}

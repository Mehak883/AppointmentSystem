using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AppointmentSystem.Models
{
    public class Slot
    {
        [Key]
        public int SlotId { get; set; }  // ✅ Fixed

        [Required]
        public int DoctorId { get; set; }  // ✅ Fixed

        [ForeignKey("DoctorId")]
        public Doctor? Doctor { get; set; }  // ✅ Navigation property

        [Required]
        public DateTime Date { get; set; }  // ✅ Fixed property name

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required]
        public SlotStatus Status { get; set; } = SlotStatus.Available;  // ✅ Fixed property

        // ✅ One-to-Many: Slot can have multiple appointments
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentSystem.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }   // ✅ Nullable to avoid EF Core errors

        [Required]
        public int SlotId { get; set; }

        [ForeignKey("SlotId")]
        public Slot? Slot { get; set; }  // ✅ Nullable to avoid EF Core errors

        [Required]
        public int SpecializationId { get; set; }

        [ForeignKey("SpecializationId")]
        public Specialization? Specialization { get; set; }

        [Required]
        public DateTime AppointDate { get; set; }

        public SlotStatus Status { get; set; } = SlotStatus.Available;
    }
}

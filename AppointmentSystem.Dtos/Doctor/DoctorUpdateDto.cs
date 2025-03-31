using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Dtos.Doctor
{
   public class DoctorUpdateDto

    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Shift start time is required")]

        public TimeSpan StartTime { get; set; }
        [Required(ErrorMessage = "Shift end time is required.")]
        public TimeSpan EndTime { get; set; }
        [Required(ErrorMessage = "Slot duration is required.")]
        [Range(15, 60, ErrorMessage = "Slot duration must be between 15 and 60 minutes.")]
        public int SlotDuration { get; set; }
        [Required(ErrorMessage = "At least one specialization is required.")]
        [MinLength(1, ErrorMessage = "At least one specialization must be selected.")]
        public List<int> SpecializationIds { get; set; } = new();
    }
    }

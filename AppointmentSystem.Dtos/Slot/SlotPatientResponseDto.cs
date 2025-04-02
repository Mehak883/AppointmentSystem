
using AppointmentSystem.Models;

namespace AppointmentSystem.Dtos.Slot
{
   public class SlotPatientResponseDto
    {
        public int SlotId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public SlotStatus Status { get; set; }
    }
}


using AppointmentSystem.Dtos.Specialization;

namespace AppointmentSystem.Dtos.Doctor
{
   public class DoctorResponseDto
    
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SlotDuration { get; set; }
        public List<SpecializationResponseDto> Specializations { get; set; } = new();
    }

}


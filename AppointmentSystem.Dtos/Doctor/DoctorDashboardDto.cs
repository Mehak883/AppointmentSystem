using AppointmentSystem.Dtos.Slot;
using AppointmentSystem.Dtos.Specialization;
using System;


namespace AppointmentSystem.Dtos.Doctor
{
  public class DoctorDashboardDto
    {
        public string UserId { get; set; }
        public int DoctorId { get; set; }
            public string Fullname { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        public List<SpecializationResponseDto> Specializations { get; set; } = new();
        public List<IGrouping<DateTime, SlotResponseDto>> SlotsGroupedByDate { get; set; }  


    }
}

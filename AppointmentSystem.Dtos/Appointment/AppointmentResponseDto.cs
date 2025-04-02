using AppointmentSystem.Models;
using System;namespace AppointmentSystem.Dtos.Appointment
{
   public class AppointmentResponseDto
    {
        public int DoctorId;
        public string DoctorName;
        public int SlotId;
        public int AppointmentId;
        public int PatientId;
        public DateTime SlotDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public SlotStatus Status { get; set; }
        public DateTime AppointDate { get; set; }

        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; }
    }
}

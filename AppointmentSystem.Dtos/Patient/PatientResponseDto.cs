using AppointmentSystem.Models;


namespace AppointmentSystem.Dtos.Patient
{
  public class PatientResponseDto
    {
        public required string UserId { get; set; }
        public int PatientId { get; set; }
        public Gender Gender { get; set; }
    }
}

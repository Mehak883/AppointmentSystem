using MediatR;

namespace AppointmentSystem.Handlers.Doctor.Command
{
   public class UpdateDoctorRequest: IRequest<bool>
    {
        public int DoctorId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SlotDuration { get; set; }
        public List<int> SpecializationIds { get; set; } = new();
    }
}

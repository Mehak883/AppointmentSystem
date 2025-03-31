using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Doctor.Command
{
    public class AddDoctorRequest : IRequest<bool>
    {
        public required string FullName { get; set; } 
        public required string Email { get; set; }
        public required string Password { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SlotDuration { get; set; }
        public List<int> SpecializationIds { get; set; } = new();
    }
}

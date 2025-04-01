using AppointmentSystem.Models;
using MediatR;


namespace AppointmentSystem.Handlers.Patient.Command
{
    public class RegisterPatientRequest : IRequest<bool>
    {
        public required string FullName { get; set; }

       
        public required string Email { get; set; }


        public required string Password { get; set; }

        public Gender Gender { get; set; }
    }
}

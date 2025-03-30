using MediatR;

namespace AppointmentSystem.Handlers.Specialization.Command
{
   public class CreateSpecializationRequest : IRequest<bool>
    {
        public required string SpecializationName { get; set; }
    }
}

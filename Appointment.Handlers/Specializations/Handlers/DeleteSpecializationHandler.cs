using AppointmentSystem.Data;
using AppointmentSystem.Handlers.Specializations.Command;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace AppointmentSystem.Handlers.Specializations.Handlers
{
    class DeleteSpecializationHandler:IRequestHandler<DeleteSpecializationRequest,bool>
    {
        private readonly AppointmentDbContext _context;

        public DeleteSpecializationHandler(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteSpecializationRequest request, CancellationToken cancellationToken)
        {
            var specialization = await _context.Specializations
                .FirstOrDefaultAsync(s => s.SpecializationId == request.SpecializationId, cancellationToken);

            if (specialization == null)
            {
                return false; 
            }

            _context.Specializations.Remove(specialization);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

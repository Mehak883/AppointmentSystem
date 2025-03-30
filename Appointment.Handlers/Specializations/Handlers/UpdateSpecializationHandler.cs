using AppointmentSystem.Data;
using AppointmentSystem.Handlers.Specializations.Command;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace AppointmentSystem.Handlers.Specializations.Handlers
{
    class UpdateSpecializationHandler : IRequestHandler<UpdateSpecializationRequest, bool>
    {
       

 private readonly AppointmentDbContext _context;

        public UpdateSpecializationHandler(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateSpecializationRequest request, CancellationToken cancellationToken)
        {
            bool exists = await _context.Specializations.AnyAsync(s => s.SpecializationName == request.SpecializationName.ToUpper(), cancellationToken);
            if (exists)
            {
                return false;
            }

            var specialization = await _context.Specializations
                .FirstOrDefaultAsync(s => s.SpecializationId == request.SpecializationId, cancellationToken);

            if (specialization == null)
            {
                return false; 
            }

            specialization.SpecializationName = request.SpecializationName.ToUpper();

            _context.Specializations.Update(specialization);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
  }

using AppointmentSystem.Data;
using AppointmentSystem.Handlers.Specialization.Command;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Handlers.Specialization.Handlers
{

        public class CreateSpecializationHandler : IRequestHandler<CreateSpecializationRequest,bool>
        {
            private readonly AppointmentDbContext _context;

            public CreateSpecializationHandler(AppointmentDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(CreateSpecializationRequest request, CancellationToken cancellationToken)
            {
            bool exists = await _context.Specializations.AnyAsync(s => s.SpecializationName == request.SpecializationName, cancellationToken);
            if (exists)
            {
                return false;
            }
            var specialization = new Models.Specialization { SpecializationName = request.SpecializationName.ToUpper() };
                _context.Specializations.Add(specialization);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }

    }
    }


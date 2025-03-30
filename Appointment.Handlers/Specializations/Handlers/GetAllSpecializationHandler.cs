using AppointmentSystem.Data;
using AppointmentSystem.Dtos.Specialization;
using AppointmentSystem.Handlers.Specializations.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace AppointmentSystem.Handlers.Specializations.Handlers
{
    class GetAllSpecializationHandler : IRequestHandler<GetAllSpecializationQuery, List<SpecializationResponseDto>>
        {
            private readonly AppointmentDbContext _context;

            public GetAllSpecializationHandler(AppointmentDbContext context)
            {
                _context = context;
            }

            public async Task<List<SpecializationResponseDto?>> Handle(GetAllSpecializationQuery request, CancellationToken cancellationToken)
            {
            List<Models.Specialization> specializations = await _context.Specializations.ToListAsync(cancellationToken);
            return specializations.Select(sp => new SpecializationResponseDto
            {
                SpecializationId = sp.SpecializationId,
                SpecializationName = sp.SpecializationName

            }).ToList();
           
            }
        }
    }



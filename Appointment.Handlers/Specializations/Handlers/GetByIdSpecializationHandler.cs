using AppointmentSystem.Data;
using AppointmentSystem.Dtos.Specialization;
using AppointmentSystem.Handlers.Specializations.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace AppointmentSystem.Handlers.Specializations.Handlers
{
    class GetByIdSpecializationHandler : IRequestHandler<GetByIdSpecializationQuery, SpecializationResponseDto>
    {
        private readonly AppointmentDbContext _context;

        public GetByIdSpecializationHandler(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<SpecializationResponseDto> Handle(GetByIdSpecializationQuery request, CancellationToken cancellationToken)
        {
            Models.Specialization specialization = await _context.Specializations
                .FirstOrDefaultAsync(s => s.SpecializationId == request.SpecializationId, cancellationToken);
            return new SpecializationResponseDto { SpecializationId = specialization.SpecializationId, SpecializationName = specialization.SpecializationName };
        }
     
    }
}

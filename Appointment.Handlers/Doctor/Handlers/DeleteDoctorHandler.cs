using AppointmentSystem.Data;
using AppointmentSystem.Handlers.Doctor.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AppointmentSystem.Handlers.Doctor.Handlers
{
    public class DeleteDoctorHandler : IRequestHandler<DeleteDoctorRequest,bool>
    {
        private readonly AppointmentDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteDoctorHandler(AppointmentDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeleteDoctorRequest request, CancellationToken cancellationToken)
        {
            var doctor = await _context.Doctors.Include(d => d.User).FirstOrDefaultAsync(d => d.DoctorId == request.DoctorId, cancellationToken);
            if (doctor == null) throw new Exception("Doctor not found");

            if (doctor.User != null)
            {
                await _userManager.DeleteAsync(doctor.User);
            }

            //_context.Doctors.Remove(doctor);
            //await _context.SaveChangesAsync();
            return true;
        }
    }

}

using AppointmentSystem.Data;
using AppointmentSystem.Handlers.Doctor.Command;
using AppointmentSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace AppointmentSystem.Handlers.Doctor.Handlers
{
    public class DeleteDoctorHandler : IRequestHandler<DeleteDoctorRequest,bool>
    {
        private readonly AppointmentDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DeleteDoctorHandler> _logger;
        public DeleteDoctorHandler(AppointmentDbContext context, UserManager<ApplicationUser> userManager, ILogger<DeleteDoctorHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteDoctorRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete doctor with ID: {DoctorId}", request.DoctorId);
            var doctor = await _context.Doctors.Include(d => d.User).FirstOrDefaultAsync(d => d.DoctorId == request.DoctorId, cancellationToken);
            if (doctor == null) {
                _logger.LogWarning("Doctor not found with ID: {DoctorId}", request.DoctorId);

                throw new Exception("Doctor not found"); }

            if (doctor.User != null)
            {

               var result= await _userManager.DeleteAsync(doctor.User);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User with ID: {UserId} deleted successfully", doctor.User.Id);
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to delete user with ID: {UserId}. Errors: {Errors}", doctor.User.Id, errors);
                    throw new Exception("Failed to delete user");
                }
            }

            return true;
        }
    }

}

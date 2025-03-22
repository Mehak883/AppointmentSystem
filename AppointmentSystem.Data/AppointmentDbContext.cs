using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Data
{
   public class AppointmentDbContext:DbContext
    {
        public AppointmentDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
    }
}

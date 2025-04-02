using AppointmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Dtos.Patient
{
  public class PatientDashboardDto
    {
        public required string UserId { get; set; }
        public int PatientId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public Gender Gender { get; set; }
    }
}

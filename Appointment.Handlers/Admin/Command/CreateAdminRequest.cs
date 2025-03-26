using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Admin.Command
{
    public class CreateAdminRequest:IRequest<bool>
    {
        public required string Fullname { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

    }
}

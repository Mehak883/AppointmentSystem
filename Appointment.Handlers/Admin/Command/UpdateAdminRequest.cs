using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Admin.Command
{
   public class UpdateAdminRequest:IRequest<bool>
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        //public string Password { get; set; }
    }
}

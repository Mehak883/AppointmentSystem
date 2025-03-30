using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Admin.Command
{
  public  class DeleteAdminRequest:IRequest<bool>
    {
        
        public required String UserId { get; set; }
    }
}

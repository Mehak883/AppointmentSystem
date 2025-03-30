using AppointmentSystem.Dtos.Admin;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.Admin.Query
{
    public class GetByIdAdminRequest:IRequest<AdminResponseDto>
    {
        public required String UserId { get; set; }

    }
}

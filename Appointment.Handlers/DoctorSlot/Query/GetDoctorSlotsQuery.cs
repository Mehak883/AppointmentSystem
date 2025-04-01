using AppointmentSystem.Dtos.Slot;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.DoctorSlot.Query
{
  public  class GetDoctorSlotsQuery:IRequest<List<SlotResponseDto>>
    {
        public int DoctorId { get; set; }

        public GetDoctorSlotsQuery(int doctorId, DateTime date)
        {
            DoctorId = doctorId;
        }
    }
}

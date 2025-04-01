using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.DoctorSlot.Command
{
   public class MarkSlotAsCompletedRequest : IRequest<bool>
    {
        public int SlotId { get; set; }

        public MarkSlotAsCompletedRequest(int slotId)
        {
            SlotId = slotId;
        }

    }
}

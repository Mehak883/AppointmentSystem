﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Handlers.DoctorSlot.Command
{
   public class GenerateTomorrowSlotsRequest : IRequest<bool> { }
    
}

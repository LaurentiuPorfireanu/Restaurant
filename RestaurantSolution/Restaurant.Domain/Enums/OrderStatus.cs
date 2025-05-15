using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Enums
{
    public enum OrderStatus
    {
        InvalidOrderState = 0,
        Registered = 1,
        InPreparation = 2,
        Delievered = 4,
        Canceled = 8
    }
}


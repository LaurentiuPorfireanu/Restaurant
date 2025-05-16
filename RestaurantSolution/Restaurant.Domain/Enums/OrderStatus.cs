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
        OutforDelivery = 4,
        Delievered = 8,
        Canceled = 16
    }
}


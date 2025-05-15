using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Enums
{
    public enum UserRole
    {
        InvalidUserType = 0,
        NotRegistered = 1,
        Registered = 2,
        Admin = 4
    }
}

using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities
{
    public class User
    {

        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }

        public string PasswordHash { get; set; }

        public UserRole Role { get; set; }


        public ICollection<Order> Orders { get; set; }
    }
}

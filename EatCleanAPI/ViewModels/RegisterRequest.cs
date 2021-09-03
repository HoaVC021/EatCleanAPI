using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatCleanAPI.ViewModels
{
    public class RegisterRequest
    {
        public string CustomerName { get; set; }
       // public string Address { get; set; }
        //public string City { get; set; }
        public string Phone { get; set; }
        //public string District { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsDisabled { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}

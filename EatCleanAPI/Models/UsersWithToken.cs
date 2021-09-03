using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatCleanAPI.Models
{
    public class UsersWithToken : User
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UsersWithToken(User customer)
        {
            this.UserId = customer.UserId;
            this.Name = customer.Name;
            this.IsAdmin = customer.IsAdmin;
            this.PhoneNumber = customer.PhoneNumber;
            this.Password = customer.Password;
            this.Email = customer.Email;
            this.IsDisabled = customer.IsDisabled;
        }
    }
}

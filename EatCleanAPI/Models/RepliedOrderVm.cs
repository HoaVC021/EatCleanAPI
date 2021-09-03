using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatCleanAPI.Models
{
    public class RepliedOrderVm
    {
        // public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

         public DateTime Time { get; set; }

        public int MyProperty { get; set; }

        public string Package { get; set; }

        public double? Price { get; set; }

        public string Paypal { get; set; }

        public string Address { get; set; }

        public string PaymentId { get; set; }
        // public DateTime CreateDate { get; set; }
        // public DateTime? LastModifiedDate { get; set; }
    }
}

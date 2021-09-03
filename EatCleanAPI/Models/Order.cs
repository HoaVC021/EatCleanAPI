using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EatCleanAPI.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public string PaymentId { get; set; }
        public string Address { get; set; }
        public string PaypalMethod { get; set; }
        public int? OrderStatus { get; set; }
        public double? ShippingPrice { get; set; }
        public double? TotalPrice { get; set; }
        public bool? IsPaid { get; set; }
        public string PaidAt { get; set; }
        public bool? IsDelivered { get; set; }
        public string DeliveredAt { get; set; }
        public string UpdateTime { get; set; }
        public string EmailAddress { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

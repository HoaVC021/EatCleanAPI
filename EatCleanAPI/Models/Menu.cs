using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EatCleanAPI.Models
{
    public partial class Menu
    {
        public Menu()
        {
            MenusDetails = new HashSet<MenusDetail>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool? IsDisabled { get; set; }

        public virtual ICollection<MenusDetail> MenusDetails { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

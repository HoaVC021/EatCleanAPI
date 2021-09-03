using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EatCleanAPI.Models
{
    public partial class Product
    {
        public Product()
        {
            MenusDetails = new HashSet<MenusDetail>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public int? Calories { get; set; }
        public float? Protein { get; set; }
        public float? Carb { get; set; }
        public float? Fat { get; set; }
        public bool? IsDisabled { get; set; }

        public virtual ICollection<MenusDetail> MenusDetails { get; set; }
    }
}

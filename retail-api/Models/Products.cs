using System;
using System.Collections.Generic;

namespace RetailApi.Models
{
    public partial class Products
    {
        public Products()
        {
            ProductOrder = new HashSet<ProductOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<ProductOrder> ProductOrder { get; set; }
    }
}

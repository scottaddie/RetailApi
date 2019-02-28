using System.Collections.Generic;

namespace RetailDomain.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductOrder = new HashSet<ProductOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<ProductOrder> ProductOrder { get; set; }
    }
}

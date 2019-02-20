using System;
using System.Collections.Generic;

namespace RetailApi.Model
{
    public partial class Orders
    {
        public Orders()
        {
            ProductOrder = new HashSet<ProductOrder>();
        }

        public int Id { get; set; }
        public DateTime OrderPlaced { get; set; }
        public DateTime? OrderFulfilled { get; set; }
        public int? CustomerId { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual ICollection<ProductOrder> ProductOrder { get; set; }
    }
}

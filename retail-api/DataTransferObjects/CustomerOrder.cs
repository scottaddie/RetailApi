using System.Collections.Generic;

namespace RetailApi.DataTransferObjects
{
    public class CustomerOrder
    {
        public string OrderPlaced { get; set; }
        public string OrderFulfilled { get; set; }
        public string CustomerName { get; set; }
        public List<OrderLineItem> OrderLineItems { get; set; }
    }
}

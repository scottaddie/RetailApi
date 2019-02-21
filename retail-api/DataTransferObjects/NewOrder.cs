using System.Collections.Generic;

namespace RetailApi.DataTransferObjects
{
    public class NewOrder
    {
        public int CustomerId { get; set; }
        public List<NewOrderLineItem> OrderLineItems { get; set; }
    }
}
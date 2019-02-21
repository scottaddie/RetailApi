using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailApi.DataTransferObjects
{
    public class NewOrder
    {
        public int CustomerId { get; set; }
        public List<NewOrderLineItem> OrderLineItems { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailApi.DataTransferObjects
{
    public class NewOrderLineItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
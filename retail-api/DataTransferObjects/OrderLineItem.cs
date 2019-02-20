using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailApi.DataTransferObjects
{
    public class OrderLineItem
    {
        public int ProductQuantity { get; set; }
        public string ProductName { get; set; }
    }
}

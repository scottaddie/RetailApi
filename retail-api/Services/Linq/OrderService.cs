using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using RetailApi.Models;
using RetailApi.DataTransferObjects;

namespace RetailApi.Services.Linq
{
    public class OrderService : IOrderService
    {
        private readonly ProductsContext _context;

        public OrderService(ProductsContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerOrder>> GetAll()
        {
            List<CustomerOrder> orders = await (
                from o in _context.Orders.AsNoTracking()
                orderby o.OrderPlaced descending
                select new CustomerOrder
                {
                    CustomerName = $"{o.Customer.LastName}, {o.Customer.FirstName}",
                    OrderFulfilled = (o.OrderFulfilled.HasValue) ? o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
                    OrderPlaced = o.OrderPlaced.ToShortDateString(),
                    OrderLineItems = (from po in o.ProductOrder
                                      select new OrderLineItem
                                      {
                                          ProductQuantity = po.Quantity,
                                          ProductName = po.Product.Name
                                      }).ToList()
                }).ToListAsync();

            return orders;
        }

        public async Task<CustomerOrder> GetById(int id)
        {
            //CustomerOrder order = await (from o in _context.Orders.AsNoTracking()
            //    where o.Id == id
            //    select new CustomerOrder
            //    {
            //        CustomerName = $"{o.Customer.LastName}, {o.Customer.FirstName}",
            //        OrderFulfilled = (o.OrderFulfilled.HasValue) ? o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
            //        OrderPlaced = o.OrderPlaced.ToShortDateString(),
            //        OrderLineItems = (from po in o.ProductOrder
            //            select new OrderLineItem
            //            {
            //                ProductQuantity = po.Quantity,
            //                ProductName = po.Product.Name
            //            }).ToList()
            //    }).FirstOrDefaultAsync();
            CustomerOrder order = await (
                from o in GetOrderById(id)
                select new CustomerOrder
                {
                    CustomerName = $"{o.Customer.LastName}, {o.Customer.FirstName}",
                    OrderFulfilled = (o.OrderFulfilled.HasValue) ? o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
                    OrderPlaced = o.OrderPlaced.ToShortDateString(),
                    OrderLineItems = (from po in o.ProductOrder
                                      select new OrderLineItem
                                      {
                                          ProductQuantity = po.Quantity,
                                          ProductName = po.Product.Name
                                      }).ToList()
                }).FirstOrDefaultAsync();

            return order;
        }

        public async Task<bool> Delete(int id)
        {
            bool isDeleted = false;
            //Orders order = await GetOrderById(id)
            //    .Include(o => o.ProductOrder)
            //    .FirstOrDefaultAsync();
            //TODO: finish writing this query
            Order order = await (
                from o in GetOrderById(id)
                join po in _context.ProductOrder.AsNoTracking() on o.Id equals po.OrderId
                select o).FirstOrDefaultAsync();

            if (order != null)
            {
                _context.Remove(order);
                await _context.SaveChangesAsync();
                isDeleted = true;
            }

            return isDeleted;
            // bool isDeleted = false;
            // Orders order = await GetOrderById(id).FirstOrDefaultAsync();

            // if (order != null)
            // {
            //     _context.RemoveRange(order.ProductOrder);
            //     _context.Remove(order);
            //     await _context.SaveChangesAsync();
            //     isDeleted = true;
            // }

            // return isDeleted;

            // bool isDeleted = false;
            // //List<ProductOrder> productOrders = await (from po in _context.ProductOrder
            // //   where po.OrderId == id
            // //   select po).ToListAsync();
            // var result = await (from po in _context.ProductOrder.AsNoTracking()
            //     join o in _context.Orders.AsNoTracking() on po.OrderId equals o.Id
            //     where po.OrderId == id
            //     select po, product).ToListAsync();

            // if (productOrders.Any())
            // {
            //    _context.RemoveRange(productOrders);
            //    _context.Remove(productOrders.FirstOrDefault().Order);
            //    await _context.SaveChangesAsync();
            //    isDeleted = true;
            // }

            // return isDeleted;
        }

        private IQueryable<Order> GetOrderById(int id) =>
            from o in _context.Orders.AsNoTracking()
            where o.Id == id
            select o;
    }
}

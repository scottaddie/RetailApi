using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetailApi.Data;
using RetailApi.DataTransferObjects;
using RetailApi.Models;

namespace RetailApi.Services.Linq
{
    public class OrderService : IOrderService
    {
        private readonly ContosoPetsContext _context;

        public OrderService(ContosoPetsContext context)
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
                    OrderId = o.Id,
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
            Order order = await GetOrderById(id)
                .Include(o => o.ProductOrder)
                .FirstOrDefaultAsync();

            if (order != null)
            {
                _context.Remove(order);
                await _context.SaveChangesAsync();
                isDeleted = true;
            }

            return isDeleted;
        }

        public async Task<Order> Create(NewOrder newOrder)
        {
            var lineItems = new List<ProductOrder>();

            foreach(var li in newOrder.OrderLineItems)
            {
                lineItems.Add(new ProductOrder
                              {
                                Quantity = li.Quantity,
                                ProductId = li.ProductId
                              });
            }

            Order order = new Order
            {
                OrderPlaced = DateTime.UtcNow,
                CustomerId = newOrder.CustomerId,
                ProductOrder = lineItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;            
        }

        public async Task<bool> SetFulfilled(int id)
        {
            bool isFulfilled = false;
            var order = await GetOrderById(id).FirstOrDefaultAsync();

            if (order != null)
            {
                order.OrderFulfilled = DateTime.UtcNow;
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                isFulfilled = true;
            }

            return isFulfilled;
        }

        private IQueryable<Order> GetOrderById(int id) =>
            from o in _context.Orders.AsNoTracking()
            where o.Id == id
            select o;
    }
}

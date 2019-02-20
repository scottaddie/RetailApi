using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetailApi.Model;
using RetailApi.DataTransferObjects;

namespace RetailApi.Services.Fluent
{
    public class OrderService : IOrderService
    {
        private readonly SomeDatabaseContext _context;

        public OrderService(SomeDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerOrder>> GetAll()
        {
            List<CustomerOrder> orders = await (_context.Orders.AsNoTracking()
                .OrderByDescending(o => o.OrderPlaced)
                .Select(o => new CustomerOrder
                {
                    CustomerName = $"{o.Customer.LastName}, {o.Customer.FirstName}",
                    OrderFulfilled =
                        (o.OrderFulfilled.HasValue) ? o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
                    OrderPlaced = o.OrderPlaced.ToShortDateString(),
                    OrderLineItems = (o.ProductOrder.Select(po =>
                            new OrderLineItem
                            {
                                ProductQuantity = po.Quantity,
                                ProductName = po.Product.Name
                            }))
                        .ToList()
                })).ToListAsync();

            return orders;
        }

        public async Task<CustomerOrder> GetById(int id)
        {
            CustomerOrder order = await GetOrderById(id)
                .Select(o => new CustomerOrder
                {
                    CustomerName = $"{o.Customer.LastName}, {o.Customer.FirstName}",
                    OrderFulfilled = (o.OrderFulfilled.HasValue) ? o.OrderFulfilled.Value.ToShortDateString() : string.Empty,
                    OrderPlaced = o.OrderPlaced.ToShortDateString(),
                    OrderLineItems = (o.ProductOrder.Select(po => new OrderLineItem
                                                            {
                                                                ProductQuantity = po.Quantity,
                                                                ProductName = po.Product.Name
                                                            })).ToList()
                }).FirstOrDefaultAsync();

            return order;
        }

        public async Task<bool> Delete(int id)
        {
            bool isDeleted = false;
            Orders order = await GetOrderById(id)
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

        private IQueryable<Orders> GetOrderById(int id) =>
            _context.Orders.AsNoTracking().Where(o => o.Id == id);
    }
}

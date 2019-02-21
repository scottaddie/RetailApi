using System.Collections.Generic;
using RetailApi.DataTransferObjects;
using System.Threading.Tasks;
using RetailApi.Models;

namespace RetailApi.Services
{
    public interface IOrderService
    {
        Task<List<CustomerOrder>> GetAll();
        Task<CustomerOrder> GetById(int id);
        Task<bool> Delete(int id);
        Task<Order> Create(NewOrder order);
    }
}

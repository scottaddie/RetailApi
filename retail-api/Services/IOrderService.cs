using System.Collections.Generic;
using RetailApi.DataTransferObjects;
using System.Threading.Tasks;

namespace RetailApi.Services
{
    public interface IOrderService
    {
        Task<List<CustomerOrder>> GetAll();
        Task<CustomerOrder> GetById(int id);
        Task<bool> Delete(int id);
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RetailApi.ViewModels;
using System.Threading.Tasks;
using RetailApi.Services;

namespace RetailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerOrder>>> Get() => 
            await _orderService.GetAll();

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerOrder>> GetById(int id) =>
            await _orderService.GetById(id);

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await _orderService.Delete(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
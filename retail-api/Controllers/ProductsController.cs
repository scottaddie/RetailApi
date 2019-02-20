using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using RetailApi.Data;
using RetailApi.Model;
//using RetailApi.Models;

namespace RetailApi.Controllers
{
    //[Route("api/{controller}", Name = "ControllerOnlyRouteName_Attribute")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SomeDatabaseContext _context;
        //private readonly LinkGenerator _linkGenerator;

        //public ProductsController(ProductsContext context,
        //                          LinkGenerator linkGenerator)
        public ProductsController(SomeDatabaseContext context)
        {
            _context = context;
            //_linkGenerator = linkGenerator;
        }

        [HttpGet]
        public ActionResult<List<Products>> GetAll() =>
            _context.Products.ToList();

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetById(long id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Products>> Create(Products product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            //var link = _linkGenerator.GetPathByAction(HttpContext, nameof(GetById), values: new { id = product.Id });

            //return Created(link, product);
            return CreatedAtAction(
                nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, Products product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
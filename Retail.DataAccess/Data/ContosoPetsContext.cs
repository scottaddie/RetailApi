using Microsoft.EntityFrameworkCore;
using Retail.Domain.Models;

namespace Retail.DataAccess.Data
{
    public partial class ContosoPetsContext : DbContext
    {
        public ContosoPetsContext()
        {
        }

        public ContosoPetsContext(DbContextOptions<ContosoPetsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductOrder> ProductOrder { get; set; }
        public virtual DbSet<Product> Products { get; set; }
    }
}

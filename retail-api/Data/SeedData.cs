using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RetailApi.Model;

namespace RetailApi.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SomeDatabaseContext(serviceProvider
                .GetRequiredService<DbContextOptions<SomeDatabaseContext>>()))
            {
                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        new Products { Name = "Squeaky Bone", Price = 20.99m },
                        new Products { Name = "Knotted Rope", Price = 12.99m }
                    );
                    context.SaveChanges();
                }

                if (!context.Customers.Any())
                {
                    context.Customers.AddRange(
                        new Customers { FirstName = "Scott", LastName = "Addie", StreetAddress = "", City = "", StateOrProvinceAbbr = "", Country = "", PostalCode = "", Phone = "", Email = "" },
                        new Customers { FirstName = "Cam", LastName = "Soper", StreetAddress = "", City = "", StateOrProvinceAbbr = "", Country = "", PostalCode = "", Phone = "", Email = "" }
                    );
                    context.SaveChanges();
                }

                if (!context.Orders.Any())
                {
                    var customer1 = context.Customers.OrderBy(c => c.Id).FirstOrDefault();
                    var customer2 = context.Customers.OrderBy(c => c.Id).LastOrDefault();

                    context.Orders.AddRange(
                        new Orders
                        {
                            OrderPlaced = DateTime.UtcNow.AddDays(-1),
                            OrderFulfilled = DateTime.UtcNow.AddHours(1),
                            Customer = customer1
                        },
                        new Orders
                        {
                            OrderPlaced = DateTime.UtcNow.AddDays(-3),
                            OrderFulfilled = DateTime.UtcNow.AddHours(4),
                            Customer = customer2
                        }
                    );

                    context.SaveChanges();
                }

                if (!context.ProductOrder.Any())
                {
                    context.ProductOrder.AddRange(
                        new ProductOrder { Order = context.Orders.OrderBy(o => o.Id).FirstOrDefault(), Product = context.Products.OrderBy(p => p.Id).First(), Quantity = 10 },
                        new ProductOrder { Order = context.Orders.OrderBy(o => o.Id).LastOrDefault(), Product = context.Products.OrderBy(p => p.Id).Last(), Quantity = 2 }
                    );

                    context.SaveChanges();
                }


            }
        }
    }
}

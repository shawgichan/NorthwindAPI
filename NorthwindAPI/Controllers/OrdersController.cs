using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI;
using NorthwindAPI.Models;
using System.Text;  // Replace with your actual namespace

namespace NorthwindAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly TestContext _context;

        public OrdersController(TestContext context)
        {
            _context = context;
        }

        [HttpGet("recent/{customerId}")]
        public async Task<IActionResult> GetRecentOrders(string customerId, string sortBy = "OrderDate")
        {
            var recentOrders = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .Take(10)
                .Select(o => new
                {
                    CustomerName = o.Customer.CompanyName,
                    o.OrderDate,
                    OrderDetails = o.OrderDetails.Select(od => new
                    {
                        ProductName = od.Product.ProductName,
                        od.UnitPrice,
                        od.Quantity,
                        od.Discount
                    })
                })
                .ToListAsync();

            var result = recentOrders.SelectMany(o => o.OrderDetails.Select(od => new
            {
                o.CustomerName,
                o.OrderDate,
                od.ProductName,
                od.UnitPrice,
                od.Quantity,
                od.Discount
            }));

            // Apply sorting
            result = sortBy.ToLower() switch
            {
                "customername" => result.OrderBy(r => r.CustomerName),
                "orderdate" => result.OrderBy(r => r.OrderDate),
                "productname" => result.OrderBy(r => r.ProductName),
                "unitprice" => result.OrderBy(r => r.UnitPrice),
                "quantity" => result.OrderBy(r => r.Quantity),
                "discount" => result.OrderBy(r => r.Discount),
                _ => result.OrderBy(r => r.OrderDate)
            };

            return Ok(result);
        }

        [HttpGet("recent/{customerId}/csv")]
        public async Task<IActionResult> GetRecentOrdersCsv(string customerId, string sortBy = "OrderDate")
        {
            var recentOrders = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .Take(10)
                .Select(o => new
                {
                    CustomerName = o.Customer.CompanyName,
                    o.OrderDate,
                    OrderDetails = o.OrderDetails.Select(od => new
                    {
                        ProductName = od.Product.ProductName,
                        od.UnitPrice,
                        od.Quantity,
                        od.Discount
                    })
                })
                .ToListAsync();

            var result = recentOrders.SelectMany(o => o.OrderDetails.Select(od => new
            {
                o.CustomerName,
                o.OrderDate,
                od.ProductName,
                od.UnitPrice,
                od.Quantity,
                od.Discount
            }));

            // Apply sorting
            result = sortBy.ToLower() switch
            {
                "customername" => result.OrderBy(r => r.CustomerName),
                "orderdate" => result.OrderBy(r => r.OrderDate),
                "productname" => result.OrderBy(r => r.ProductName),
                "unitprice" => result.OrderBy(r => r.UnitPrice),
                "quantity" => result.OrderBy(r => r.Quantity),
                "discount" => result.OrderBy(r => r.Discount),
                _ => result.OrderBy(r => r.OrderDate)
            };

            // Convert to CSV
            var csv = new StringBuilder();
            csv.AppendLine("CustomerName,OrderDate,ProductName,UnitPrice,Quantity,Discount");
            foreach (var item in result)
            {
                csv.AppendLine($"{item.CustomerName},{item.OrderDate:yyyy-MM-dd},{item.ProductName},{item.UnitPrice},{item.Quantity},{item.Discount}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "recent_orders.csv");
        }
    }
}
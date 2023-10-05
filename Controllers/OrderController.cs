using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorPos.Data;

namespace BlazorPos.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController : Controller {
    private readonly ProductStoreContext _db;

    public OrderController(ProductStoreContext db) {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetOrders() {
        var orders = await _db.Orders
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .ToListAsync();

        return orders;    
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<List<Order>>> GetOrders(string username) {
        var orders = await _db.Orders
            .Where(o => o.CreatedBy == username)
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .ToListAsync();

        return orders;    
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateOrder(Order Order) {
        Order newOrder = new Order();
        newOrder.CreatedTime = DateTime.Now;
        newOrder.CreatedBy = Order.CreatedBy;

        foreach (var orderProduct in Order.OrderProducts) {
            newOrder.OrderProducts.Add(orderProduct);
            orderProduct.Order = newOrder;
            Console.WriteLine(orderProduct.Product.Description);
            Console.WriteLine("Product Id: " + orderProduct.ProductId);
        }

        _db.Orders.Attach(newOrder);
        await _db.SaveChangesAsync();

        return newOrder.OrderId;
    }
} 
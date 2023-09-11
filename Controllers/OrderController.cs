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
        return (await _db.Orders.ToListAsync()).OrderByDescending(o => o.CreatedTime).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateOrder(Order newOrder) {
        newOrder.CreatedTime = DateTime.Now;

        _db.Orders.Attach(newOrder);
        await _db.SaveChangesAsync();

        return newOrder.OrderId;
    }
} 
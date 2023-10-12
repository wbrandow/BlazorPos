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
            .OrderByDescending(o => o.CreatedTime)
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

        var productIds = Order.OrderProducts.Select(op => op.ProductId).ToList();

        var products = await _db.Products
            .Where(p => productIds.Contains(p.Id))
            .Include(p => p.InventoryItems)
            .ToListAsync();

        foreach (var orderProduct in Order.OrderProducts) {

            var product = products.FirstOrDefault(p => p.Id == orderProduct.ProductId);
            
            product.InventoryItems = orderProduct.Product.InventoryItems;

            var newOrderProduct = new OrderProduct() {
                OrderId = orderProduct.OrderId,
                ProductId = orderProduct.ProductId,
                QtyOnOrder = orderProduct.QtyOnOrder,
                OrderProductPrice = orderProduct.OrderProductPrice,
                Tax = orderProduct.Tax,
                LineDiscount = orderProduct.LineDiscount
            };

            newOrder.OrderProducts.Add(newOrderProduct);
        }

        _db.Orders.Attach(newOrder);
        await _db.SaveChangesAsync();

        return newOrder.OrderId;
    }
} 
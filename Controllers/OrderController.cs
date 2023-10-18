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
    public async Task<ActionResult<List<Sale>>> GetOrders() {
        var sales = await _db.Sales
            .Include(s => s.SaleLines)
                .ThenInclude(line => line.Product)
            .ToListAsync();

        return sales;    
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<List<Sale>>> GetOrders(string username) {
        var sales = await _db.Sales
            .Where(s => s.Salesperson == username)
            .OrderByDescending(s => s.TimeOfSale)
            .Include(s => s.SaleLines)
                .ThenInclude(line => line.Product)
            .ToListAsync();

        return sales;    
    }

    [HttpGet("sales-by-product")]
    public async Task<ActionResult<List<Sale>>> GetSalesByProductId(int productId) {
        var sales = await _db.Sales
            .Where(s => s.SaleLines.Any(line => line.ProductId == productId))
            .Include(s => s.SaleLines)
            .ToListAsync();

        return sales;    
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateOrder(Sale Sale) {
        Sale newSale = new Sale();
        newSale.TimeOfSale = DateTime.Now;
        newSale.Salesperson = Sale.Salesperson;

        var productIds = Sale.SaleLines.Select(line => line.ProductId).ToList();

        var products = await _db.Products
            .Where(p => productIds.Contains(p.Id))
            .Include(p => p.InventoryItems)
            .ToListAsync();

        foreach (var line in Sale.SaleLines) {

            var product = products.FirstOrDefault(p => p.Id == line.ProductId);
            
            product.InventoryItems = line.Product.InventoryItems;

            var newLine = new SaleLine() {
                SaleId = line.SaleId,
                ProductId = line.ProductId,
                Units = line.Units,
                UnitSalePrice = line.UnitSalePrice,
                UnitCost = line.UnitCost,
                ApplyTax = line.ApplyTax,
                LineDiscount = line.LineDiscount
            };

            newSale.SaleLines.Add(newLine);
        }

        _db.Sales.Attach(newSale);
        await _db.SaveChangesAsync();

        return newSale.Id;
    }
} 

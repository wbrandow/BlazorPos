using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorPos.Data;
using System.ComponentModel;

namespace BlazorPos.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController : Controller {
    private readonly ProductStoreContext _db;

    public ProductController(ProductStoreContext db) {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts() {
        return (await _db.Products.ToListAsync()).OrderByDescending(p => p.Price).ToList();
    }
    
    [HttpGet("{ProductId}")]
    public async Task<ActionResult<Product>> GetProduct(int ProductId) {
        var product = await _db.Products
            .Where(p => p.Id == ProductId)
            .SingleOrDefaultAsync();

        if (product == null) {
            return NotFound();
        }

        return product;    
    }

        [HttpGet("results")]
        public async Task<ActionResult<List<Product>>> GetResults(string searchField, string searchValue) {
            if (searchField == "Description") {
                return await _db.Products
                    .Where(p => p.Description == searchValue)
                    .ToListAsync();
            }
            else if (searchField == "Brand") {
                return await _db.Products
                    .Where(p => p.Brand == searchValue)
                    .ToListAsync();
            }
            else if (searchField == "Vendor") {
                return await _db.Products
                    .Where(p => p.Vendor == searchValue)
                    .ToListAsync();
            }
            else {
                return await _db.Products
                    .Where(p => p.Description == searchValue || p.Brand == searchValue || p.Vendor == searchValue)
                    .ToListAsync();
            }
        }    







    [HttpPut("{ProductId}")]
    public async Task<ActionResult<Product>> UpdateProduct(int ProductId, Product updatedProduct) {
        var existingProduct = await _db.Products
            .Where(p => p.Id == ProductId)
            .SingleOrDefaultAsync();

        if (existingProduct == null) {
            return NotFound();
        }

        existingProduct.UPC = updatedProduct.UPC;
        existingProduct.EAN = updatedProduct.EAN;
        existingProduct.SKU = updatedProduct.SKU;
        existingProduct.Description = updatedProduct.Description;
        existingProduct.Brand = updatedProduct.Brand;
        existingProduct.Vendor = updatedProduct.Vendor;
        existingProduct.Price = updatedProduct.Price;
        existingProduct.MSRP = updatedProduct.MSRP;
        existingProduct.OnlinePrice = updatedProduct.OnlinePrice;
        existingProduct.DefaultCost = updatedProduct.DefaultCost;

        try {
            _db.Update(existingProduct);
            await _db.SaveChangesAsync();
            return existingProduct;
        }
        catch (DbUpdateConcurrencyException) {
            return Conflict();
        }
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateProduct(Product newProduct) {
        _db.Products.Attach(newProduct);
        await _db.SaveChangesAsync();

        return newProduct.Id;
    }

    [HttpDelete("{ProductId}")]
    public async Task<ActionResult<bool>> DeleteProduct(int ProductId) {
        var product = await _db.Products
        .Where(p => p.Id == ProductId)
        .SingleOrDefaultAsync();

        if (product == null) {
            return NotFound();
        }

        try {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex) {
            Console.Error.WriteLine(ex);
            return false;
        }
    }
}
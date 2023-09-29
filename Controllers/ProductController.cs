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
            switch (searchField) {
                case "Description":
                    return await _db.Products
                        .Where(p => p.Description.Contains(searchValue))
                        .ToListAsync(); 
                
                case "Brand":
                    return await _db.Products
                        .Where(p => p.Brand.Contains(searchValue))
                        .ToListAsync();
                    
                case "Vendor":
                    return await _db.Products
                        .Where(p => p.Vendor.Contains(searchValue))
                        .ToListAsync();  

                case "UPC":
                    return await _db.Products
                        .Where(p => p.UPC.Contains(searchValue))
                        .ToListAsync();

                case "EAN":
                    return await _db.Products
                        .Where(p => p.EAN.Contains(searchValue))
                        .ToListAsync(); 

                case "SKU":
                    return await _db.Products
                        .Where(p => p.SKU.Contains(searchValue))
                        .ToListAsync();               

                default:
                    return await _db.Products
                        .Where(p => p.Description.Contains(searchValue) 
                            || p.Brand.Contains(searchValue) 
                            || p.Vendor.Contains(searchValue)
                            || p.UPC.Contains(searchValue)
                            || p.EAN.Contains(searchValue)
                            || p.SKU.Contains(searchValue))
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
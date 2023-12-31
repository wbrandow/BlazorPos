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
        return await _db.Products
            .Include(p => p.InventoryItems)
            .Include(p => p.TaxClass)
                .ThenInclude(tc => tc.TaxRates)
            .AsSplitQuery()
            .ToListAsync();
    }
    
    [HttpGet("{ProductId}")]
    public async Task<ActionResult<Product>> GetProduct(int ProductId) {
        var product = await _db.Products
            .Where(p => p.Id == ProductId)
            .Include(p => p.InventoryItems)
            .Include(p => p.TaxClass)
                .ThenInclude(tc => tc.TaxRates)
            .AsSplitQuery()    
            .SingleOrDefaultAsync();

        if (product == null) {
            return NotFound();
        }

        return product;    
    }

        [HttpGet("results")]
        public async Task<ActionResult<List<Product>>> GetResults(string searchField, string searchValue) {
            searchValue = searchValue.ToLower();

            switch (searchField) {
                case "Description":
                    return await _db.Products
                        .Where(p => p.Description.ToLower().Contains(searchValue))
                        .Include(p => p.InventoryItems)
                        .Include(p => p.TaxClass)
                            .ThenInclude(tc => tc.TaxRates)
                        .AsSplitQuery()
                        .ToListAsync(); 
                
                case "Brand":
                    return await _db.Products
                        .Where(p => p.Brand.ToLower().Contains(searchValue))
                        .Include(p => p.InventoryItems)
                        .Include(p => p.TaxClass)
                            .ThenInclude(tc => tc.TaxRates)
                        .AsSplitQuery()    
                        .ToListAsync();
                    
                case "Vendor":
                    return await _db.Products
                        .Where(p => p.Vendor.ToLower().Contains(searchValue))
                        .Include(p => p.InventoryItems)
                        .Include(p => p.TaxClass)
                            .ThenInclude(tc => tc.TaxRates)
                        .AsSplitQuery()    
                        .ToListAsync();  

                case "UPC":
                    return await _db.Products
                        .Where(p => p.UPC.ToLower().Contains(searchValue))
                        .Include(p => p.InventoryItems)
                        .Include(p => p.TaxClass)
                            .ThenInclude(tc => tc.TaxRates)
                        .AsSplitQuery()    
                        .ToListAsync();

                case "EAN":
                    return await _db.Products
                        .Where(p => p.EAN.ToLower().Contains(searchValue))
                        .Include(p => p.InventoryItems)
                        .Include(p => p.TaxClass)
                            .ThenInclude(tc => tc.TaxRates)
                        .AsSplitQuery()    
                        .ToListAsync(); 

                case "SKU":
                    return await _db.Products
                        .Where(p => p.SKU.ToLower().Contains(searchValue))
                        .Include(p => p.InventoryItems)
                        .Include(p => p.TaxClass)
                            .ThenInclude(tc => tc.TaxRates)
                        .AsSplitQuery()    
                        .ToListAsync();               

                default:
                    return await _db.Products
                        .Where(p => p.Description.ToLower().Contains(searchValue) 
                            || p.Brand.ToLower().Contains(searchValue) 
                            || p.Vendor.ToLower().Contains(searchValue)
                            || p.UPC.ToLower().Contains(searchValue)
                            || p.EAN.ToLower().Contains(searchValue)
                            || p.SKU.ToLower().Contains(searchValue))
                        .Include(p => p.InventoryItems) 
                        .Include(p => p.TaxClass)
                            .ThenInclude(tc => tc.TaxRates) 
                        .AsSplitQuery()      
                        .ToListAsync();
            }
        }    

    [HttpPut("{ProductId}")]
    public async Task<ActionResult<Product>> UpdateProduct(int ProductId, Product updatedProduct) {
        var existingProduct = await _db.Products
            .Where(p => p.Id == ProductId)
            .Include(p => p.InventoryItems)
            .Include(p => p.TaxClass)
                .ThenInclude(tc => tc.TaxRates)
            .AsSplitQuery()    
            .SingleOrDefaultAsync();

        if (existingProduct == null) {
            return NotFound();
        }

        Console.WriteLine("Existing product Tax Class: " + existingProduct.TaxClass.TaxClassName);
        

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
        existingProduct.TaxClassId = updatedProduct.TaxClassId;
        existingProduct.TaxClass = await _db.TaxClasses.Include(tc => tc.TaxRates).Where(tc => tc.Id == updatedProduct.TaxClassId).FirstOrDefaultAsync();
        existingProduct.InventoryItems = updatedProduct.InventoryItems;

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
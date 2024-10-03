using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorPos.Data;
using System.ComponentModel;

namespace BlazorPos.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomerController : Controller {
    private readonly ProductStoreContext _db;

    public CustomerController(ProductStoreContext db) {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Customer>>> GetCustomers() {
        return await _db.Customers
            .Include(c => c.Sales)
            .ToListAsync();
    }

    [HttpGet("{CustomerId}")]
    public async Task<ActionResult<Customer>> GetCustomer(int CustomerId) {
        var customer = await _db.Customers
            .Where(c => c.Id == CustomerId)
            .Include(c => c.Sales)
                .ThenInclude(s => s.SaleLines)
                    .ThenInclude(line => line.Product)
                        .ThenInclude(p => p.TaxClass)
                            .ThenInclude(tc => tc.TaxRates)
            .AsSplitQuery()    
            .SingleOrDefaultAsync();

        if (customer == null) {
            return NotFound();
        }

        return customer;   
    }    

    [HttpGet("results")]
    public async Task<ActionResult<List<Customer>>> GetResults(string searchField, string searchValue) {
        searchValue = searchValue.ToLower();

        switch (searchField) {
            case "Phone":
                return await _db.Customers
                    .Where(c => c.Phone.ToLower().Contains(searchValue))
                    .ToListAsync(); 
                
            case "Email":
                return await _db.Customers
                    .Where(c => c.Email.ToLower().Contains(searchValue))  
                    .ToListAsync();
                    
            case "LastName":
                return await _db.Customers
                    .Where(c => c.LastName.ToLower().Contains(searchValue))    
                    .ToListAsync();   

            case "Company":
                return await _db.Customers
                    .Where(c => c.Company.ToLower().Contains(searchValue))    
                    .ToListAsync();               

            default:
                return await _db.Customers
                    .Where(c => c.Phone.ToLower().Contains(searchValue) 
                        || c.Email.ToLower().Contains(searchValue) 
                        || c.LastName.ToLower().Contains(searchValue)
                        || c.Company.ToLower().Contains(searchValue))   
                    .ToListAsync();
        }
    }

    [HttpPut("{CustomerId}")]
    public async Task<ActionResult<Customer>> UpdateCustomer(int CustomerId, Customer updatedCustomer) {
        var existingCustomer = await _db.Customers
            .Where(c => c.Id == CustomerId)   
            .SingleOrDefaultAsync();

        if (existingCustomer == null) {
            return NotFound();
        }

        existingCustomer.Type = updatedCustomer.Type;
        existingCustomer.FirstName = updatedCustomer.FirstName;
        existingCustomer.LastName = updatedCustomer.LastName;
        existingCustomer.Title = updatedCustomer.Title;
        existingCustomer.Company = updatedCustomer.Company;
        existingCustomer.Birthday = updatedCustomer.Birthday;
        existingCustomer.Email = updatedCustomer.Email;
        existingCustomer.Phone = updatedCustomer.Phone;
        existingCustomer.AlternatePhone = updatedCustomer.AlternatePhone;
        existingCustomer.AddressLine1 = updatedCustomer.AddressLine1;
        existingCustomer.AddressLine2 = updatedCustomer.AddressLine2;
        existingCustomer.City = updatedCustomer.City;
        existingCustomer.ZipCode = updatedCustomer.ZipCode;
        existingCustomer.State = updatedCustomer.State;

        try {
            _db.Update(existingCustomer);
            await _db.SaveChangesAsync();
            return existingCustomer;
        }
        catch (DbUpdateConcurrencyException) {
            return Conflict();
        }
    }  

    [HttpPut("removeSale")]
    public async Task<ActionResult> RemoveSale(Sale sale) {
        Customer customer = await _db.Customers
            .Where(c => c.Id == sale.CustomerId)
                .Include(c => c.Sales)
            .FirstOrDefaultAsync();

        if (customer == null) {
            return NotFound();
        }

        customer.Sales.RemoveAll(s => s.Id == sale.Id);

        try {
            _db.Update(customer);
            await _db.SaveChangesAsync();
            return Ok();
        }
        catch (DbUpdateConcurrencyException) {
            return Conflict();
        }
    }

    [HttpPut("addSale")]
    public async Task<ActionResult> AddSale(Sale sale) {
        Customer customer = await _db.Customers
            .Where(c => c.Id == sale.CustomerId)
                .Include(c => c.Sales)
                    .ThenInclude(s => s.SaleLines)
                        .ThenInclude(sl => sl.Product)
                            .ThenInclude(p => p.TaxClass)
                                .ThenInclude(tc => tc.TaxRates)
            .FirstOrDefaultAsync();

        if (customer == null) {
            return NotFound();
        }

        foreach (var line in sale.SaleLines) {
            // Optionally check if product and tax class are valid or already exist
            var existingProduct = await _db.Products
                .Include(p => p.TaxClass) // Include TaxClass if you need its details
                .SingleOrDefaultAsync(p => p.Id == line.Product.Id);
            
            if (existingProduct != null) {
                line.Product = existingProduct; // Associate the tracked product
            }
        }
        
        customer.Sales.Add(sale);

        try {
            _db.Update(customer);
            await _db.SaveChangesAsync();
            return Ok();
        }
        catch (DbUpdateConcurrencyException) {
            return Conflict();
        }
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateCustomer(Customer newCustomer) {
        try {
            _db.Customers.Add(newCustomer);
            await _db.SaveChangesAsync();
            return newCustomer.Id;
        }
        catch (DbUpdateException) {
            return StatusCode(500, "A database error occurred.");  // Handle other database update errors
        } 
    }

    [HttpPost("{CustomerId}")]
    public async Task<ActionResult<int>> AddSaleWithLines(int CustomerId, Sale newSale) {
        var existingCustomer = await _db.Customers
            .Include(c => c.Sales)
                .ThenInclude(s => s.SaleLines) // Load SaleLines for sale lines handling
            .SingleOrDefaultAsync(c => c.Id == CustomerId);

        if (existingCustomer == null) {
            return NotFound();
        }

        foreach (var line in newSale.SaleLines) {
            // Optionally check if product and tax class are valid or already exist
            var existingProduct = await _db.Products
                .Include(p => p.TaxClass) // Include TaxClass if you need its details
                .SingleOrDefaultAsync(p => p.Id == line.Product.Id);
            
            if (existingProduct != null) {
                line.Product = existingProduct; // Associate the tracked product
            }
        }

        existingCustomer.Sales.Add(newSale);

        try {
            await _db.SaveChangesAsync();
            return newSale.Id;
        }
        catch (DbUpdateConcurrencyException) {
            return Conflict();
        }
        catch (DbUpdateException) {
            return StatusCode(500, "A database error occurred.");
        }
    }

    [HttpDelete("{CustomerId}")]
    public async Task<ActionResult<bool>> DeleteCustomer(int CustomerId) {
        var customer = await _db.Customers
        .Where(c => c.Id == CustomerId)
        .Include(c => c.Sales)
        .AsSplitQuery()
        .SingleOrDefaultAsync();

        if (customer == null) {
            return NotFound();
        }

        try {
            foreach (var sale in customer.Sales) {
                sale.CustomerId = null;
                sale.Customer = null;
            }

            await _db.SaveChangesAsync();

            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex) {
            Console.Error.WriteLine(ex);
            return false;
        }
    }

}
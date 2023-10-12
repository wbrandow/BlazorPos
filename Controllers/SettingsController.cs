using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorPos.Data;
using System.ComponentModel;

namespace BlazorPos.Controllers;

[Route("api/settings")]
[ApiController]
public class SettingsController : Controller {
    private readonly ProductStoreContext _db;

    public SettingsController(ProductStoreContext db) {
        _db = db;
    }

    [HttpGet("taxclasses")]
    public async Task<ActionResult<List<TaxClass>>> GetTaxClasses() {
        return await _db.TaxClasses
            .Include(tc => tc.TaxRates)
            .ToListAsync();
    }

    [HttpGet("taxrates")]
    public async Task<ActionResult<List<TaxRate>>> GetTaxRates() {
        return await _db.TaxRates.ToListAsync();
    }
}
    
using Microsoft.EntityFrameworkCore;

namespace BlazorPos.Data;

public class ProductStoreContext : DbContext {
    public ProductStoreContext(DbContextOptions options) : base(options) {}

    public DbSet<Order> Orders { get; set; }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }
}
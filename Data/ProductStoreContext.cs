using Microsoft.EntityFrameworkCore;

namespace BlazorPos.Data;

public class ProductStoreContext : DbContext {
    public ProductStoreContext(DbContextOptions options) : base(options) {}

    public DbSet<Order> Orders { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<OrderProduct> OrderProducts { get; set; }

    public DbSet<TaxRate> TaxRates { get; set; }

    public DbSet<TaxClass> TaxClasses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany()
            .HasForeignKey(op => op.ProductId);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.TaxClass)
            .WithMany()
            .HasForeignKey(p => p.TaxClassId);

        modelBuilder.Entity<TaxClass>()
            .HasMany(tc => tc.TaxRates)
            .WithMany()
            .UsingEntity(junction => junction.ToTable("TaxClassTaxRate"));        

    }
}
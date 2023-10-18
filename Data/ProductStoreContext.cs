using Microsoft.EntityFrameworkCore;

namespace BlazorPos.Data;

public class ProductStoreContext : DbContext {
    public ProductStoreContext(DbContextOptions options) : base(options) {}

    public DbSet<Sale> Sales { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<SaleLine> SaleLines { get; set; }

    public DbSet<TaxRate> TaxRates { get; set; }

    public DbSet<TaxClass> TaxClasses { get; set; }

    public DbSet<InventoryItem> InventoryItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InventoryItem>()
            .HasOne<Product>()
            .WithMany(p => p.InventoryItems)
            .HasForeignKey(item => item.ProductId);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.TaxClass)
            .WithMany()
            .HasForeignKey(p => p.TaxClassId);

        modelBuilder.Entity<TaxClass>()
            .HasMany(tc => tc.TaxRates)
            .WithMany()
            .UsingEntity(junction => junction.ToTable("TaxClassTaxRate")); 

        modelBuilder.Entity<SaleLine>()
            .HasOne(line => line.Product)
            .WithMany()
            .HasForeignKey(line => line.ProductId);

        modelBuilder.Entity<SaleLine>()
            .HasOne(line => line.Sale)
            .WithMany(s => s.SaleLines)
            .HasForeignKey(line => line.SaleId);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Customer)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.CustomerId) 
            .IsRequired(false);

        modelBuilder.Entity<Payment>()
            .HasOne<Sale>()
            .WithMany(s => s.Payments)
            .HasForeignKey(p => p.SaleId);   
    }
}
namespace BlazorPos.Data;

public static class SeedData {
    public static void Initialize(ProductStoreContext db) {
         var taxRates = new TaxRate[] {
            new TaxRate() {
                Id = 1,
                TaxRateName = "PA Sales Tax",
                Rate = 6.0m
            },
            new TaxRate() {
                Id = 2,
                TaxRateName = "Local Tax",
                Rate = 2.0m
            },
            new TaxRate() {
                Id = 3,
                TaxRateName = "PA Food Tax",
                Rate = 0.0m
            }            
        };

        db.TaxRates.AddRange(taxRates);
        db.SaveChanges();


        var taxClasses = new TaxClass[] {
            new TaxClass() {
                Id = 1,
                TaxClassName = "Product",
                TaxRates = new List<TaxRate> { taxRates[0], taxRates[1] }
            },
            new TaxClass() {
                Id = 2,
                TaxClassName = "Food",
                TaxRates = new List<TaxRate> { taxRates[2] }
            }
        };

        db.TaxClasses.AddRange(taxClasses);
        db.SaveChanges();

        var products = new Product[] {
            new Product() {
                Id = 1,
                UPC = "123456789876",
                EAN = "0123456789876",
                SKU = "0123456789876",
                Description = "Example Item 1",
                Brand = "Example Brand",
                Vendor = "Example Vendor",
                Price = 9.99m,
                MSRP = 10.00m,
                OnlinePrice = 9.95m,
                DefaultCost = 5.99m,
                TaxClassId = 1,
                TaxClass = taxClasses[0]
            },
            new Product() {
                Id = 2,
                UPC = "223456789876",
                EAN = "0223456789876",
                SKU = "0223456789876",
                Description = "Example Item 2",
                Brand = "Example Brand",
                Vendor = "Example Vendor",
                Price = 19.99m,
                MSRP = 20.00m,
                OnlinePrice = 19.95m,
                DefaultCost = 9.99m,
                TaxClassId = 1,
                TaxClass = taxClasses[0]
            },
            new Product() {
                Id = 3,
                UPC = "323456789876",
                EAN = "0323456789876",
                SKU = "0323456789876",
                Description = "Example Food Item 3",
                Brand = "Example Brand",
                Vendor = "Example Vendor",
                Price = 29.99m,
                MSRP = 30.00m,
                OnlinePrice = 29.95m,
                DefaultCost = 15.99m,
                TaxClassId = 2,
                TaxClass = taxClasses[1]
            }
        };

        db.Products.AddRange(products);
        db.SaveChanges();
    }
}
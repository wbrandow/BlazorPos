namespace BlazorPos.Data;

public static class SeedData {
    public static void Initialize(ProductStoreContext db) {
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
            },
            new Product() {
                Id = 3,
                UPC = "323456789876",
                EAN = "0323456789876",
                SKU = "0323456789876",
                Description = "Example Item 3",
                Brand = "Example Brand",
                Vendor = "Example Vendor",
                Price = 29.99m,
                MSRP = 30.00m,
                OnlinePrice = 29.95m,
                DefaultCost = 15.99m
            }
        };

        db.Products.AddRange(products);
        db.SaveChanges();
    }
}
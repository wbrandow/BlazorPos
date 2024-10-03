namespace BlazorPos.Services;

public class OrderState {
    public Sale Sale { get; private set; } = new Sale();

    public SaleLine SaleLine { get; set; }

    public OrderState() {
        Sale = new Sale();
    }

    public void SelectProduct(Product product) {
        if (Sale.SaleLines.Any(line => line.ProductId == product.Id)) {
            var saleLineToUpdate = Sale.SaleLines.FirstOrDefault(line => line.ProductId == product.Id);
            saleLineToUpdate.Units++;
        }
        else {
            SaleLine = new SaleLine() {
                SaleId = Sale.Id,
                Sale = Sale,
                ProductId = product.Id,
                Product = product,
                UnitSalePrice = product.Price,
                Units = 1
            };

            Sale.SaleLines.Add(SaleLine);
            SaleLine = null;
        }
    }

    public void ReturnSale(Sale returnSale) {
        Sale.CustomerId = returnSale.CustomerId;
        Sale.Customer = returnSale.Customer;

        foreach (var line in returnSale.SaleLines) {
            SaleLine newLine = new SaleLine() {
                SaleId = Sale.Id,
                Sale = Sale,
                ProductId = line.ProductId,
                Product = line.Product,
                UnitSalePrice = line.UnitSalePrice,
                Units = -line.Units,
                UnitCost = line.UnitCost,
                LineDiscount = line.LineDiscount,
                ApplyTax = line.ApplyTax
            };

            Sale.SaleLines.Add(newLine);
            newLine = null;
        }
    }

    public void RemoveProduct(int productId) {
        SaleLine = new SaleLine();

        foreach (var line in Sale.SaleLines) {
            if (line.ProductId == productId) {
                SaleLine = line;
            }
        }

        Sale.SaleLines.Remove(SaleLine);
        
        SaleLine = null;
    }

    public void SelectCustomer(Customer customer) {
        Sale.CustomerId = customer.Id;
        Sale.Customer = customer;
    }

    public void RemoveCustomer() {
        Sale.CustomerId = null;
        Sale.Customer = null;
    }

    public void ResetOrder() {
        Sale = new Sale();
    }
}

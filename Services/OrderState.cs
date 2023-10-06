namespace BlazorPos.Services;

public class OrderState {
    public Order Order { get; private set; } = new Order();

    public OrderProduct OrderProduct { get; set; }

    public OrderState() {
        Order = new Order();
    }

    public void SelectProduct(Product product) {
        if (Order.OrderProducts.Any(op => op.ProductId == product.Id)) {
            var orderProductToUpdate = Order.OrderProducts.FirstOrDefault(op => op.ProductId == product.Id);
            orderProductToUpdate.QtyOnOrder++;
        }
        else {
            OrderProduct = new OrderProduct() {
            OrderId = Order.OrderId,
            Order = Order,
            ProductId = product.Id,
            Product = product,
            OrderProductPrice = product.Price,
            QtyOnOrder = 1,
            };

            Order.OrderProducts.Add(OrderProduct);
            OrderProduct = null;
        }
    }

    public void RemoveProduct(int productId) {
        OrderProduct = new OrderProduct();

        foreach (var op in Order.OrderProducts) {
            if (op.ProductId == productId) {
                OrderProduct = op;
            }
        }

        Order.OrderProducts.Remove(OrderProduct);
        
        OrderProduct = null;
    }

    public void ResetOrder() {
        Order = new Order();
    }
}

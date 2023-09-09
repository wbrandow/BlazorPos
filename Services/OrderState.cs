namespace BlazorPos.Services;

public class OrderState {
    public Order Order { get; private set; } = new Order();

    public OrderState() {
        Order = new Order();
    }

    public void SelectProduct(Product product) {
        Order.Products.Add(product);
    }

    public void RemoveProduct(Product product) {
        Order.Products.Remove(product);
    }

    public void ResetOrder() {
        Order = new Order();
    }
}

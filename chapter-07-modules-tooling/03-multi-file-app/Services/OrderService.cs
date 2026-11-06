using MultiFileApp.Models;

namespace MultiFileApp.Services;

public class OrderService
{
    private readonly List<Order> _orders = new();

    public IReadOnlyList<Order> All => _orders;

    public void Add(Order order) => _orders.Add(order);

    public decimal Revenue() => _orders.Sum(o => o.Total);
}

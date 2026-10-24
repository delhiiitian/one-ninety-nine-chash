namespace MultiFileApp.Models;   // file-scoped namespace: applies to the whole file

public record OrderLine(string Product, int Quantity, decimal UnitPrice)
{
    public decimal Total => Quantity * UnitPrice;
}

public record Order(int Id, string Customer, List<OrderLine> Lines)
{
    public decimal Total => Lines.Sum(l => l.Total);
}

// 5.6 Custom exceptions
// Run: dotnet run chapter-05-error-handling/06-custom-exceptions.cs

// Your own exception types name problems from your domain and carry extra data.
var stock = new Dictionary<string, int> { ["laptop"] = 2, ["phone"] = 10 };

TryOrder(stock, "laptop", 1);
TryOrder(stock, "laptop", 5);
TryOrder(stock, "tablet", 1);

static void TryOrder(Dictionary<string, int> stock, string product, int quantity)
{
    try
    {
        PlaceOrder(stock, product, quantity);
        Console.WriteLine($"Ordered {quantity} x {product}. {stock[product]} left.");
    }
    catch (OutOfStockException ex)
    {
        Console.WriteLine($"Sorry: {ex.Message} You could order {ex.Available} instead.");
    }
    catch (ProductNotFoundException ex)
    {
        Console.WriteLine($"Sorry: {ex.Message}");
    }
}

static void PlaceOrder(Dictionary<string, int> stock, string product, int quantity)
{
    if (!stock.TryGetValue(product, out int available))
        throw new ProductNotFoundException(product);
    if (quantity > available)
        throw new OutOfStockException(product, quantity, available);
    stock[product] = available - quantity;
}

// A base class for all shop errors lets callers catch the whole family at once.
class ShopException : Exception
{
    public ShopException(string message) : base(message) { }
}

class ProductNotFoundException : ShopException
{
    public string Product { get; }
    public ProductNotFoundException(string product)
        : base($"We do not sell '{product}'.") => Product = product;
}

class OutOfStockException : ShopException
{
    public string Product { get; }
    public int Requested { get; }
    public int Available { get; }

    public OutOfStockException(string product, int requested, int available)
        : base($"Only {available} {product}(s) in stock, you asked for {requested}.")
    {
        Product = product;
        Requested = requested;
        Available = available;
    }
}

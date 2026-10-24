// 1.2 Variables and the var keyword
// Run: dotnet run chapter-01-basics/02-variables.cs

// A variable is a named box that holds a value of one type.
string customerName = "Asha";
int itemsInCart = 3;
double cartTotal = 59.97;
bool isMember = true;

Console.WriteLine($"Customer: {customerName}");
Console.WriteLine($"Items:    {itemsInCart}");
Console.WriteLine($"Total:    {cartTotal}");
Console.WriteLine($"Member:   {isMember}");

// var lets the compiler work out the type from the value on the right.
var city = "Pune";          // string
var orderNumber = 1042;     // int
Console.WriteLine($"{city} order #{orderNumber}");

// Variables can change (that is why they are called variables).
itemsInCart = itemsInCart + 1;
cartTotal += 12.50;
Console.WriteLine($"After adding a book: {itemsInCart} items, total {cartTotal}");

// The type never changes: this line would not compile.
// itemsInCart = "four";

// Declare first, assign later. C# will not let you read it before it has a value.
int deliveryDays;
deliveryDays = isMember ? 1 : 3;
Console.WriteLine($"Delivery in {deliveryDays} day(s)");

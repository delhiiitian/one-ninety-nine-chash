// 7.3 Splitting code across files and folders
// Run: dotnet run --project chapter-07-modules-tooling/03-multi-file-app
//
// Convention: folder name = namespace part. Models/Order.cs -> MultiFileApp.Models.
using MultiFileApp.Models;
using MultiFileApp.Services;

var service = new OrderService();
service.Add(new Order(1, "Asha", [new OrderLine("Tea", 2, 120m), new OrderLine("Biscuits", 3, 45m)]));
service.Add(new Order(2, "Ben", [new OrderLine("Coffee", 1, 380m)]));

foreach (Order order in service.All)
    Console.WriteLine($"Order {order.Id} for {order.Customer}: {order.Total:N2}");

Console.WriteLine($"Revenue: {service.Revenue():N2}");
Console.WriteLine($"Order type lives in namespace: {typeof(Order).Namespace}");

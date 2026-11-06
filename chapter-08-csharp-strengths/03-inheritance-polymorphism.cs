// 8.3 Inheritance and polymorphism
// Run: dotnet run chapter-08-csharp-strengths/03-inheritance-polymorphism.cs

// One list of the base type can hold every kind of employee.
List<Employee> staff =
[
    new SalariedEmployee("Meera", monthlySalary: 90_000m),
    new HourlyEmployee("Ravi", hourlyRate: 450m, hoursWorked: 160),
    new SalesEmployee("Lena", monthlySalary: 40_000m, sales: 1_200_000m),
];

decimal payroll = 0;
foreach (Employee e in staff)
{
    // Polymorphism: the same call runs a different method for each type.
    decimal pay = e.MonthlyPay();
    payroll += pay;
    Console.WriteLine($"{e.Describe(),-38} pays {pay,10:N2}");
}
Console.WriteLine($"{"Total payroll",-38}      {payroll,10:N2}");

// is / as / pattern matching on types.
foreach (Employee e in staff)
    if (e is SalesEmployee s) Console.WriteLine($"{s.Name} earned commission on {s.Sales:N0} of sales");

// abstract: cannot be created directly, only through a child class.
abstract class Employee(string name)
{
    public string Name { get; } = name;

    // abstract: every child MUST provide this.
    public abstract decimal MonthlyPay();

    // virtual: children MAY replace this.
    public virtual string Describe() => $"{Name} ({GetType().Name.Replace("Employee", "")})";
}

class SalariedEmployee(string name, decimal monthlySalary) : Employee(name)
{
    public decimal MonthlySalary { get; } = monthlySalary;
    public override decimal MonthlyPay() => MonthlySalary;
}

class HourlyEmployee(string name, decimal hourlyRate, int hoursWorked) : Employee(name)
{
    public override decimal MonthlyPay()
    {
        int overtime = Math.Max(0, hoursWorked - 160);
        return (hoursWorked - overtime) * hourlyRate + overtime * hourlyRate * 1.5m;
    }
    public override string Describe() => $"{base.Describe()}, {hoursWorked} h";
}

// sealed: nobody can inherit from this one. It extends SalariedEmployee.
sealed class SalesEmployee(string name, decimal monthlySalary, decimal sales) : SalariedEmployee(name, monthlySalary)
{
    public decimal Sales { get; } = sales;
    public override decimal MonthlyPay() => base.MonthlyPay() + Sales * 0.02m;   // base + 2% commission
}

// 1.5 Booleans and characters
// Run: dotnet run chapter-01-basics/05-bool-char.cs

// bool holds true or false: the answer to a yes/no question.
int age = 19;
bool hasTicket = true;
bool isAdult = age >= 18;
bool canEnter = isAdult && hasTicket;
Console.WriteLine($"Adult: {isAdult}, has ticket: {hasTicket}, can enter: {canEnter}");

// Logical operators: && (and), || (or), ! (not).
bool isWeekend = false;
bool isHoliday = true;
bool dayOff = isWeekend || isHoliday;
Console.WriteLine($"Day off: {dayOff}, working day: {!dayOff}");

// char holds a single character in single quotes.
char grade = 'A';
char initial = "Meera"[0];
Console.WriteLine($"Grade {grade}, initial {initial}");

// char helpers are great for checking user input.
string password = "Sun2024!";
int digits = 0, letters = 0, symbols = 0;
foreach (char c in password)
{
    if (char.IsDigit(c)) digits++;
    else if (char.IsLetter(c)) letters++;
    else symbols++;
}
Console.WriteLine($"Password has {letters} letters, {digits} digits, {symbols} symbols");
bool strong = password.Length >= 8 && digits > 0 && symbols > 0;
Console.WriteLine($"Strong password: {strong}");

// Characters are numbers underneath (Unicode code points).
char letter = 'C';
Console.WriteLine($"'{letter}' has code {(int)letter}; the next letter is '{(char)(letter + 1)}'");
Console.WriteLine($"Upper of 'q' is '{char.ToUpper('q')}'");

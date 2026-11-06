// 4.6 Queue<T> and Stack<T>
// Run: dotnet run chapter-04-collections/06-queue-stack.cs

// Queue: first in, first out (FIFO), like a line at a ticket counter.
var printQueue = new Queue<string>();
printQueue.Enqueue("report.pdf");
printQueue.Enqueue("photo.jpg");
printQueue.Enqueue("invoice.docx");
Console.WriteLine($"Next to print: {printQueue.Peek()} ({printQueue.Count} waiting)");
while (printQueue.Count > 0)
{
    string job = printQueue.Dequeue();
    Console.WriteLine($"Printing {job}");
}

// Stack: last in, first out (LIFO), like a pile of plates. Perfect for undo.
var undo = new Stack<string>();
string text = "";
foreach (string word in new[] { "Hello", " brave", " new", " world" })
{
    undo.Push(text);                    // save the state before each change
    text += word;
    Console.WriteLine($"Typed: \"{text}\"");
}
for (int i = 0; i < 2; i++)
{
    text = undo.Pop();
    Console.WriteLine($"Undo:  \"{text}\"");
}

// A stack checks balanced brackets, the way code editors do.
Console.WriteLine($"(a[b]{{c}}) balanced? {IsBalanced("(a[b]{c})")}");
Console.WriteLine($"(a[b)] balanced?    {IsBalanced("(a[b)]")}");

// PriorityQueue: the smallest priority comes out first (hospital triage).
var triage = new PriorityQueue<string, int>();
triage.Enqueue("sprained ankle", 3);
triage.Enqueue("chest pain", 1);
triage.Enqueue("cough", 5);
while (triage.TryDequeue(out string? patient, out int priority))
    Console.WriteLine($"Treat (priority {priority}): {patient}");

static bool IsBalanced(string input)
{
    var open = new Stack<char>();
    foreach (char c in input)
    {
        if (c is '(' or '[' or '{') open.Push(c);
        else if (c is ')' or ']' or '}')
        {
            if (open.Count == 0) return false;
            char expected = c switch { ')' => '(', ']' => '[', _ => '{' };
            if (open.Pop() != expected) return false;
        }
    }
    return open.Count == 0;
}

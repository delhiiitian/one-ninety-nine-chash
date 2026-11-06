// 9.7 Databases with SQLite and ADO.NET
// Run: dotnet run chapter-09-real-world/07-sqlite.cs
#:package Microsoft.Data.Sqlite@10.0.14
#:property PublishAot=false

using Microsoft.Data.Sqlite;

// 1. A SQLite database is a single file. Start fresh each run.
string dbPath = Path.Combine(Path.GetTempPath(), "c199-shop.db");
if (File.Exists(dbPath)) File.Delete(dbPath);

using var connection = new SqliteConnection($"Data Source={dbPath}");
connection.Open();

// 2. Create a table with a plain SQL command.
Execute(connection, """
    CREATE TABLE products (
        id       INTEGER PRIMARY KEY AUTOINCREMENT,
        name     TEXT    NOT NULL,
        price    REAL    NOT NULL,
        stock    INTEGER NOT NULL DEFAULT 0
    );
    """);

// 3. Insert rows with PARAMETERS. Never glue user input into SQL text (SQL injection).
//    A transaction makes the batch all-or-nothing and much faster.
using (SqliteTransaction tx = connection.BeginTransaction())
{
    using SqliteCommand insert = connection.CreateCommand();
    insert.CommandText = "INSERT INTO products (name, price, stock) VALUES ($name, $price, $stock)";
    var name = insert.Parameters.Add("$name", SqliteType.Text);
    var price = insert.Parameters.Add("$price", SqliteType.Real);
    var stock = insert.Parameters.Add("$stock", SqliteType.Integer);
    foreach (var (n, p, s) in new[] { ("Notebook", 45.0, 120), ("Gel pen", 15.0, 300), ("Stapler", 180.0, 12), ("Backpack", 899.0, 0) })
    {
        name.Value = n; price.Value = p; stock.Value = s;
        insert.ExecuteNonQuery();
    }
    tx.Commit();
}

// 4. Read rows with a data reader.
Console.WriteLine("All products:");
using (SqliteCommand query = connection.CreateCommand())
{
    query.CommandText = "SELECT id, name, price, stock FROM products ORDER BY price";
    using SqliteDataReader reader = query.ExecuteReader();
    while (reader.Read())
        Console.WriteLine($"  #{reader.GetInt32(0)} {reader.GetString(1),-9} Rs {reader.GetDouble(2),6:0.00}  stock {reader.GetInt32(3)}");
}

// 5. A single value with ExecuteScalar.
using (SqliteCommand count = connection.CreateCommand())
{
    count.CommandText = "SELECT COUNT(*) FROM products WHERE stock = 0";
    Console.WriteLine($"Out of stock: {count.ExecuteScalar()}");
}

// 6. Update with a parameter; ExecuteNonQuery returns the number of rows changed.
using (SqliteCommand update = connection.CreateCommand())
{
    update.CommandText = "UPDATE products SET price = price * 0.9 WHERE price > $min";
    update.Parameters.AddWithValue("$min", 100);
    Console.WriteLine($"10% discount applied to {update.ExecuteNonQuery()} product(s)");
}

// 7. Parameters keep hostile input harmless: this finds nothing instead of dumping the table.
string userSearch = "x' OR '1'='1";
using (SqliteCommand search = connection.CreateCommand())
{
    search.CommandText = "SELECT COUNT(*) FROM products WHERE name = $name";
    search.Parameters.AddWithValue("$name", userSearch);
    Console.WriteLine($"Search for \"{userSearch}\" matched {search.ExecuteScalar()} row(s)");
}

connection.Close();
SqliteConnection.ClearAllPools();
File.Delete(dbPath);

static void Execute(SqliteConnection c, string sql)
{
    using SqliteCommand cmd = c.CreateCommand();
    cmd.CommandText = sql;
    cmd.ExecuteNonQuery();
}

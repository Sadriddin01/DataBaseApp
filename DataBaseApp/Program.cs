using DateBaseSQL.Metods;
using Newtonsoft.Json;

public static class Program
{
    static void Main(string[] args)
    {

        File.WriteAllText("appsettings.json", "{\n  \"ConnectionString\": {\n    \"PgConnection\": \"\"\n  }\n}");

        //*onsole.BackgroundColor = ConsoleColor.Green;*/
        //Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Host (localhost): ");
        string host = Console.ReadLine();

        Console.Write("Port (5432): ");
        string port = Console.ReadLine();

        Console.Write("Database Name (Name): ");
        string database = Console.ReadLine();

        Console.Write("User ID (postgres): ");
        string userId = Console.ReadLine();

        Console.Write("Password: ");
        string password = Console.ReadLine();
        //Console.BackgroundColor = ConsoleColor.Green;
        //Console.ForegroundColor = ConsoleColor.Black;
        Console.Clear();
        string connectionString = $"Host={host};Port={port};Database={database};User Id={userId};Password={password};";


        var appSettings = new
        {
            ConnectionString = new
            {
                PgConnection = connectionString
            }
        };

        string json = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
        File.WriteAllText("appsettings.json", json);

        var loadedSettings = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText("appsettings.json"));
        string pgConnection = loadedSettings.ConnectionString.PgConnection;

        //Console.WriteLine("Nima kerak: ");
        //string Category = Console.ReadLine();
        //var query = $"SELECT * FROM {Category}";
        //Select.Selects(pgConnection, query);

        //Select.GetTableNames(connectionString);


        bool exit = false;
        int selectedIndex = 0;
        Console.WriteLine("-------------Schemas-------------");
        List<string> Buyruqlar = new List<string>
        {
            "Create Table",
            "List Table",
            "Alter Table",
            "Table Sturucture",
            "What's in the Table",
            "Delete Table"
        };

        while (!exit)
        {
            Console.Clear();
            for (int i = 0; i < Buyruqlar.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine(Buyruqlar[i]);
                Console.ResetColor();
            }

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1) % Buyruqlar.Count;
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex - 1 + Buyruqlar.Count) % Buyruqlar.Count;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                switch (selectedIndex)
                {
                    case 0:
                        Console.WriteLine("Creating Table");
                        Select.CreateTable(connectionString);
                        break;
                    case 1:
                        Console.WriteLine("Table Lists");
                        Select.GetTableNames(connectionString);
                        break;
                    case 2:
                        Console.WriteLine("Alter Table");
                        Select.AlterTable(connectionString);
                        break;
                    case 3:
                        Console.WriteLine("Table Structure");
                        Select.TableStructure(connectionString);
                        break;
                    case 4:
                        Console.WriteLine("In the table: ");
                        Select.GetAbout(connectionString);
                        break;
                    case 5:
                        Console.WriteLine("table DELETING");
                        Select.DeleteTable(connectionString);
                        break;
                }
                Console.ReadKey();
            }
        }
        Console.Clear();
    }
}

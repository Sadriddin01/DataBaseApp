using Npgsql;
using System;
using System.Data;

namespace DateBaseSQL.Methods
{
    public partial class Select
    {
        public static void GetTableNames(string connectionString)
        {
            Console.Clear();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var schema = connection.GetSchema("Tables");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Tables in the database:");
                    foreach (DataRow table in schema.Rows)
                    {
                        Console.WriteLine(table["TABLE_NAME"]);
                    }
                    Console.ResetColor();

                    Console.Write("\nEnter the name of the table you want to display: ");
                    string tableName = Console.ReadLine();

                    if (NotExist(connectionString, tableName))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("There is no table with that name!");
                        Console.ResetColor();
                    }
                    else
                    {
                        GetTableData(connectionString, tableName);
                        GetTableColumns(connectionString, tableName); 
                    }
                }
            }
            catch (NpgsqlException npgEx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Npgsql error: {npgEx.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static void GetTableColumns(string connectionString, string tableName)
        {
            Console.Clear();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var schema = connection.GetSchema("Columns", new string[] { null, null, tableName, null });

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nColumns in the '{tableName}' table:");
                    foreach (DataRow column in schema.Rows)
                    {
                        Console.WriteLine(column["COLUMN_NAME"]);
                    }
                    Console.ResetColor();
                }
            }
            catch (NpgsqlException npgEx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Npgsql error: {npgEx.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static bool NotExist(string connectionString, string tableName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var schema = connection.GetSchema("Tables");

                    foreach (DataRow row in schema.Rows)
                    {
                        if (row["TABLE_NAME"].ToString().Equals(tableName, StringComparison.OrdinalIgnoreCase))
                        {
                            return false; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.ResetColor();
            }

            return true; 
        }

        public static void GetTableData(string connectionString, string tableName)
        {
            Console.Clear();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand($"SELECT * FROM \"{tableName}\"", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader.GetName(i) + "\t");
                        }
                        Console.WriteLine();
                        Console.ResetColor();

                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader.GetValue(i) + "\t");
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (NpgsqlException npgEx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Npgsql error: {npgEx.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}

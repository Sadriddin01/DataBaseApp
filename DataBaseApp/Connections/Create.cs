using System;
using System.Collections.Generic;
using DateBaseSQL.Methods;
using Npgsql;

namespace DataBaseApp.Connections
{
    public partial class Create
    {
        public static void CreateTable(string connectionString)
        {
            Console.Clear();
            Select.GetTableNames(connectionString);

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    Console.Write("Enter the table name: ");
                    string tableName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(tableName))
                    {
                        Console.WriteLine("Table name cannot be empty.");
                        return;
                    }

                    List<string> columns = new List<string>();
                    bool addMoreColumns = true;

                    while (addMoreColumns)
                    {
                        Console.Write("Enter the column name: ");
                        string columnName = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(columnName))
                        {
                            Console.WriteLine("Column name cannot be empty.");
                            continue;
                        }

                        string dataType = GetColumnType();
                        if (string.IsNullOrEmpty(dataType))
                        {
                            Console.WriteLine("Invalid data type selection. Please try again.");
                            continue;
                        }

                        columns.Add($"{columnName} {dataType}");

                        Console.Write("Do you want to add another column? (y/n): ");
                        string YesOrNo = Console.ReadLine();
                        addMoreColumns = YesOrNo?.Trim().ToLower() == "y";
                    }

                    if (columns.Count == 0)
                    {
                        Console.WriteLine("No columns were defined. Table creation aborted.");
                        return;
                    }

                    string createTableQuery = $"CREATE TABLE IF NOT EXISTS {tableName} (\n" +
                                               string.Join(",\n", columns) +
                                               "\n);";

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = createTableQuery;
                        command.ExecuteNonQuery();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Table '{tableName}' was created successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private static string GetColumnType()
        {
            while (true)
            {
                Console.WriteLine("Select the column type:");
                Console.WriteLine("1. VARCHAR(255)");
                Console.WriteLine("2. INTEGER");
                Console.WriteLine("3. SERIAL");
                Console.WriteLine("4. DECIMAL(10, 2)");
                Console.Write("Enter the number corresponding to your choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        return "VARCHAR(255)";
                    case "2":
                        return "INTEGER";
                    case "3":
                        return "SERIAL";
                    case "4":
                        return "DECIMAL(10, 2)";
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid column type.");
                        break;
                }
            }
        }
    }
}

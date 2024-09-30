using DateBaseSQL.Methods;
using Npgsql;
using System;
using System.Collections.Generic;

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

                    List<string> columns = new List<string>();
                    bool addMoreColumns = true;

                    while (addMoreColumns)
                    {
                        Console.Write("Enter the column name: ");
                        string columnName = Console.ReadLine();

                        Console.WriteLine("Select the column type:");
                        Console.WriteLine("1. VARCHAR(255)");
                        Console.WriteLine("2. INTEGER");
                        Console.WriteLine("3. SERIAL");
                        Console.WriteLine("4. DECIMAL(10, 2)");
                        Console.Write("Enter the number corresponding to your choice: ");
                        string dataType = "";

                        switch (Console.ReadLine())
                        {
                            case "1":
                                dataType = "VARCHAR(255)";
                                break;
                            case "2":
                                dataType = "INTEGER";
                                break;
                            case "3":
                                dataType = "SERIAL";
                                break;
                            case "4":
                                dataType = "DECIMAL(10, 2)";
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Defaulting to VARCHAR(255).");
                                dataType = "VARCHAR(255)";
                                break;
                        }

                        columns.Add($"{columnName} {dataType}");

                        Console.Write("Do you want to add another column? (y/n): ");
                        string YesOrNo = Console.ReadLine();
                        addMoreColumns = YesOrNo.ToLower() == "y";
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

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.ResetColor();
            }
        }
    }
}

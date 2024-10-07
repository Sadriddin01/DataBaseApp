using Npgsql;
using System;

namespace DataBaseApp.Connections
{
    public partial class Deleting
    {
        public static void DeleteTable(string connectionString)
        {
            Console.Clear();
            try
            {
                Console.Write("Enter the name of the table you want to delete: ");
                string tableName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(tableName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Table name cannot be empty.");
                    Console.ResetColor();
                    return;
                }

                Console.Write($"Are you sure you want to delete the table '{tableName}'? (y/n): ");
                string confirmation = Console.ReadLine()?.Trim().ToLower();

                if (confirmation == "y")
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = $"DROP TABLE IF EXISTS \"{tableName}\"";
                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            command.ExecuteNonQuery();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Table '{tableName}' deleted successfully.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Table deletion canceled.");
                }
            }
            catch (NpgsqlException npgEx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Npgsql error: {npgEx.Message}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}

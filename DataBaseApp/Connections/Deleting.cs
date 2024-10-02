using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                Console.Write($"Are you sure you want to delete the table '{tableName}'? (y/n): ");
                string confirmation = Console.ReadLine().ToLower();

                if (confirmation == "y")
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = $"DROP TABLE IF EXISTS \"{tableName}\"";
                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Table '{tableName}' deleted successfully.");
                        }

                        connection.Close();
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
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }

    }
}

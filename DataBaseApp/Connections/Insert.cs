using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseApp.Connections
{
    public partial class Insert
    {
        public static void InsertIntoColumn(string connectionString)
        {
            Console.Clear();
            try
            {
                Console.Write("Enter the name of the table: ");
                string tableName = Console.ReadLine();

                Console.Write("Enter the name of the column: ");
                string columnName = Console.ReadLine();

                Console.Write("Enter the value to insert: ");
                string value = Console.ReadLine();

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"INSERT INTO \"{tableName}\" (\"{columnName}\") VALUES (@value)";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value", value);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{rowsAffected} row(s) added successfully to {tableName}.");
                    }

                    connection.Close();
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

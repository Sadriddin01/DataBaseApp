using DateBaseSQL.Methods;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DataBaseApp.Connections
{
    public partial class Insert
    {
        public static void InsertIntoColumn(string connectionString)
        {
            Console.Clear();
            try
            {
                Select.GetTableNames(connectionString);
                Console.Write("Enter the name of the table: ");
                string tableName = Console.ReadLine();

                if (Select.NotExist(connectionString, tableName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"The table '{tableName}' does not exist!");
                    Console.ResetColor();
                    return; 
                }

                Console.Write("Enter the name of the column: ");
                string columnName = Console.ReadLine();

                if (!ColumnExists(connectionString, tableName, columnName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"The column '{columnName}' does not exist in the table '{tableName}'.");
                    Console.ResetColor();
                    return; 
                }

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
                        Console.WriteLine($"{rowsAffected} row(s) added successfully to '{tableName}'.");
                    }
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

        private static bool ColumnExists(string connectionString, string tableName, string columnName)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "SELECT COUNT(*) FROM information_schema.columns WHERE table_name = @tableName AND column_name = @columnName", connection))
                {
                    command.Parameters.AddWithValue("@tableName", tableName);
                    command.Parameters.AddWithValue("@columnName", columnName);
                    return (long)command.ExecuteScalar() > 0;
                }
            }
        }
    }
}

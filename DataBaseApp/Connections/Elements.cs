using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using DateBaseSQL.Methods;

namespace DataBaseApp.Connections
{
    public partial class Elements
    {
        public static void Elementss(string connectionString)
        {
            // Retrieve and display available table names
            Select.GetTableNames(connectionString);
            Console.WriteLine("Choose the table: ");
            string tableName = Console.ReadLine();

            // Check if the selected table exists
            if (Select.NotExist(connectionString, tableName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The table with that name does not exist!");
                Console.ResetColor();
            }
            else
            {
                // Display data from the selected table
                Select.GetTableData(connectionString, tableName);
            }
        }

        public static void GetColumnData(string connectionString, string tableName, string columnName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $"SELECT * FROM \"{tableName}\"";  // Retrieve all data from the table
                        using (var reader = command.ExecuteReader())
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"\nData in the column '{columnName}' of table '{tableName}':");
                            while (reader.Read())
                            {
                                Console.WriteLine(reader[columnName].ToString());  // Print each value in the specified column
                            }
                            Console.ResetColor();
                        }
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

        public static void UpdateData(string connectionString, string tableName, string conditionColumn, object conditionValue, Dictionary<string, object> newData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Create the SQL UPDATE query
                    var setClauses = string.Join(", ", newData.Keys.Select(key => $"\"{key}\" = @{key}"));
                    string query = $"UPDATE \"{tableName}\" SET {setClauses} WHERE \"{conditionColumn}\" = @conditionValue";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        // Add parameters to the query
                        foreach (var entry in newData)
                        {
                            command.Parameters.AddWithValue($"@{entry.Key}", entry.Value);
                        }
                        command.Parameters.AddWithValue("@conditionValue", conditionValue);

                        // Execute the query and print how many rows were updated
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} rows updated.");
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

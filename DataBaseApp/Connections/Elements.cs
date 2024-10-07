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
            Select.GetTableNames(connectionString);
            Console.WriteLine("Choose the table: ");
            string tableName = Console.ReadLine();

            if (Select.NotExist(connectionString, tableName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The table with that name does not exist!");
                Console.ResetColor();
                return; 
            }

            Select.GetTableData(connectionString, tableName);
        }

        public static void GetColumnData(string connectionString, string tableName, string columnName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    if (!ColumnExists(connection, tableName, columnName))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"The column '{columnName}' does not exist in the table '{tableName}'.");
                        return; 
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $"SELECT \"{columnName}\" FROM \"{tableName}\""; 
                        using (var reader = command.ExecuteReader())
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"\nData in the column '{columnName}' of table '{tableName}':");
                            while (reader.Read())
                            {
                                Console.WriteLine(reader[columnName]?.ToString()); 
                            }
                        }
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

        public static void UpdateData(string connectionString, string tableName, string conditionColumn, object conditionValue, Dictionary<string, object> newData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    var setClauses = string.Join(", ", newData.Keys.Select(key => $"\"{key}\" = @{key}"));
                    string query = $"UPDATE \"{tableName}\" SET {setClauses} WHERE \"{conditionColumn}\" = @conditionValue";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        foreach (var entry in newData)
                        {
                            command.Parameters.AddWithValue($"@{entry.Key}", entry.Value);
                        }
                        command.Parameters.AddWithValue("@conditionValue", conditionValue);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} rows updated.");
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

        private static bool ColumnExists(NpgsqlConnection connection, string tableName, string columnName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT COUNT(*) FROM information_schema.columns WHERE table_name = @tableName AND column_name = @columnName";
                command.Parameters.AddWithValue("@tableName", tableName);
                command.Parameters.AddWithValue("@columnName", columnName);

                return (long)command.ExecuteScalar() > 0; 
            }
        }
    }
}

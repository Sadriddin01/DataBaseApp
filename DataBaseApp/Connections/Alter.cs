using System;
using DateBaseSQL.Methods;
using Npgsql;

namespace DataBaseApp.Connections
{
    public partial class Alter
    {
        public static void AlterTable(string connectionString)
        {
            try
            {
                Select.GetTableNames(connectionString);
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    Console.Write("Choose the table you want to alter: ");
                    string tableName = Console.ReadLine();

                    Console.WriteLine("\nSelect one of the following options to alter the table:");
                    Console.WriteLine("1. Add Column");
                    Console.WriteLine("2. Delete Column");
                    Console.WriteLine("3. Rename Column");
                    Console.WriteLine("4. Rename Table");
                    Console.WriteLine("5. Change Column Type");

                    string option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            AddColumn(connection, tableName);
                            break;

                        case "2":
                            DeleteColumn(connection, tableName);
                            break;

                        case "3":
                            RenameColumn(connection, tableName);
                            break;

                        case "4":
                            RenameTable(connection, tableName);
                            break;

                        case "5":
                            ChangeColumnType(connection, tableName);
                            break;

                        default:
                            Console.WriteLine("Invalid option selected.");
                            break;
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
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static void AddColumn(NpgsqlConnection connection, string tableName)
        {
            Console.Write("Enter the name of the column to add: ");
            string newColumnName = Console.ReadLine();

            string dataType = GetColumnType();
            if (string.IsNullOrEmpty(dataType))
            {
                return;
            }

            string addColumnQuery = $"ALTER TABLE {tableName} ADD COLUMN {newColumnName} {dataType};";
            ExecuteNonQuery(connection, addColumnQuery);
            Console.WriteLine($"Column '{newColumnName}' added successfully.");
        }

        private static void DeleteColumn(NpgsqlConnection connection, string tableName)
        {
            Console.Write("Enter the name of the column to delete: ");
            string dropColumnName = Console.ReadLine();

            string dropColumnQuery = $"ALTER TABLE {tableName} DROP COLUMN {dropColumnName};";
            ExecuteNonQuery(connection, dropColumnQuery);
            Console.WriteLine($"Column '{dropColumnName}' deleted successfully.");
        }

        private static void RenameColumn(NpgsqlConnection connection, string tableName)
        {
            Console.Write("Enter the current column name: ");
            string oldColumnName = Console.ReadLine();

            Console.Write("Enter the new column name: ");
            string newColumnName = Console.ReadLine();

            string renameColumnQuery = $"ALTER TABLE {tableName} RENAME COLUMN {oldColumnName} TO {newColumnName};";
            ExecuteNonQuery(connection, renameColumnQuery);
            Console.WriteLine($"Column '{oldColumnName}' renamed to '{newColumnName}' successfully.");
        }

        private static void RenameTable(NpgsqlConnection connection, string tableName)
        {
            Console.Write("Enter the new table name: ");
            string newTableName = Console.ReadLine();

            string renameTableQuery = $"ALTER TABLE {tableName} RENAME TO {newTableName};";
            ExecuteNonQuery(connection, renameTableQuery);
            Console.WriteLine($"Table '{tableName}' renamed to '{newTableName}' successfully.");
        }

        private static void ChangeColumnType(NpgsqlConnection connection, string tableName)
        {
            Console.Write("Enter the name of the column to change its type: ");
            string columnToChange = Console.ReadLine();

            string newDataType = GetColumnType();
            if (string.IsNullOrEmpty(newDataType))
            {
                return;
            }

            string changeColumnTypeQuery = $"ALTER TABLE {tableName} ALTER COLUMN {columnToChange} TYPE {newDataType};";
            ExecuteNonQuery(connection, changeColumnTypeQuery);
            Console.WriteLine($"Column '{columnToChange}' type changed to '{newDataType}' successfully.");
        }

        private static string GetColumnType()
        {
            Console.WriteLine("Select the data type:");
            Console.WriteLine("1. VARCHAR(255)");
            Console.WriteLine("2. INTEGER");
            Console.Write("Enter the corresponding number: ");

            switch (Console.ReadLine())
            {
                case "1":
                    return "VARCHAR(255)";
                case "2":
                    return "INTEGER";
                default:
                    Console.WriteLine("Invalid choice.");
                    return string.Empty;
            }
        }

        private static void ExecuteNonQuery(NpgsqlConnection connection, string query)
        {
            using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}

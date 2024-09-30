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
                    Select.GetTableNames(connectionString);
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
                            Console.Clear();
                            Console.Write("Enter the name of the column to add: ");
                            string newColumnName = Console.ReadLine();

                            Console.WriteLine("Select the data type of the column:");
                            Console.WriteLine("1. VARCHAR(255)");
                            Console.WriteLine("2. INTEGER");
                            Console.Write("Enter the corresponding number: ");
                            string dataType = "";

                            switch (Console.ReadLine())
                            {
                                case "1":
                                    dataType = "VARCHAR(255)";
                                    break;
                                case "2":
                                    dataType = "INTEGER";
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Defaulting to VARCHAR(255).");
                                    dataType = "VARCHAR(255)";
                                    break;
                            }

                            string addColumnQuery = $"ALTER TABLE {tableName} ADD COLUMN {newColumnName} {dataType};";
                            ExecuteNonQuery(connection, addColumnQuery);
                            Console.WriteLine($"Column '{newColumnName}' added successfully.");
                            break;

                        case "2":
                            Console.Clear();
                            Console.Write("Enter the name of the column to delete: ");
                            string dropColumnName = Console.ReadLine();

                            string dropColumnQuery = $"ALTER TABLE {tableName} DROP COLUMN {dropColumnName};";
                            ExecuteNonQuery(connection, dropColumnQuery);
                            Console.WriteLine($"Column '{dropColumnName}' deleted successfully.");
                            break;

                        case "3":
                            Console.Clear();
                            Console.Write("Enter the current column name: ");
                            string oldColumnName = Console.ReadLine();

                            Console.Write("Enter the new column name: ");
                            string newColumnNameForRename = Console.ReadLine();

                            string renameColumnQuery = $"ALTER TABLE {tableName} RENAME COLUMN {oldColumnName} TO {newColumnNameForRename};";
                            ExecuteNonQuery(connection, renameColumnQuery);
                            Console.WriteLine($"Column '{oldColumnName}' renamed to '{newColumnNameForRename}' successfully.");
                            break;

                        case "4":
                            Console.Clear();
                            Console.Write("Enter the new table name: ");
                            string newTableName = Console.ReadLine();

                            string renameTableQuery = $"ALTER TABLE {tableName} RENAME TO {newTableName};";
                            ExecuteNonQuery(connection, renameTableQuery);
                            Console.WriteLine($"Table '{tableName}' renamed to '{newTableName}' successfully.");
                            break;

                        case "5":
                            Console.Clear();
                            Console.Write("Enter the name of the column to change its type: ");
                            string columnToChange = Console.ReadLine();

                            Console.WriteLine("Select the new data type:");
                            Console.WriteLine("1. VARCHAR(255)");
                            Console.WriteLine("2. INTEGER");
                            Console.Write("Enter the corresponding number: ");
                            string newDataType = "";

                            switch (Console.ReadLine())
                            {
                                case "1":
                                    newDataType = "VARCHAR(255)";
                                    break;
                                case "2":
                                    newDataType = "INTEGER";
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Defaulting to VARCHAR(255).");
                                    newDataType = "VARCHAR(255)";
                                    break;
                            }

                            string changeColumnTypeQuery = $"ALTER TABLE {tableName} ALTER COLUMN {columnToChange} TYPE {newDataType};";
                            ExecuteNonQuery(connection, changeColumnTypeQuery);
                            Console.WriteLine($"Column '{columnToChange}' type changed to '{newDataType}' successfully.");
                            break;

                        default:
                            Console.WriteLine("Invalid option selected.");
                            break;
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

        private static void ExecuteNonQuery(NpgsqlConnection connection, string query)
        {
            using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}

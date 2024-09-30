using Npgsql;
using System.Data;

namespace DateBaseSQL.Methods
{
    public partial class Select
    {
        public static void GetTableNames(string connectionString)
        {
            Console.Clear();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        var schema = connection.GetSchema("Tables");
                        foreach (DataRow table in schema.Rows)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(table["TABLE_NAME"]);
                        }
                    }

                    Console.Write("\nEnter the name of the table you want to display: ");
                    string tableName = Console.ReadLine();
                    GetTableData(connectionString, tableName);

                    Console.Write("\nWhich table's columns do you want to view?: ");
                    string columnName = Console.ReadLine();
                    if (NotExist(connectionString, tableName))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("There is no table with that name!");
                        Console.ResetColor();
                    }
                    else
                    {
                        GetTableColumns(connectionString, tableName);
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
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static void GetTableColumns(string connectionString, string tableName)
        {
            Console.Clear();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        var schema = connection.GetSchema("Columns", new string[] { null, null, tableName, null });
                        Console.WriteLine($"\nColumns in the '{tableName}' table:");
                        foreach (DataRow column in schema.Rows)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(column["COLUMN_NAME"]);
                        }
                        Console.ResetColor();
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
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static bool NotExist(string connectionString, string tableName)
        {
            bool tableNotExist = true;
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var schema = connection.GetSchema("Tables");

                    foreach (DataRow row in schema.Rows)
                    {
                        if (row["TABLE_NAME"].ToString().Equals(tableName, StringComparison.OrdinalIgnoreCase))
                        {
                            tableNotExist = false;
                            break;
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.ResetColor();
            }

            return tableNotExist;
        }

        public static void GetTableData(string connectionString, string tableName)
        {
            Console.Clear();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"SELECT * FROM \"{tableName}\"";

                        using (var reader = command.ExecuteReader())
                        {
                            // Display column names
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader.GetName(i) + "\t");
                            }
                            Console.WriteLine();
                            Console.ResetColor();

                            // Display rows of data
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.Write(reader.GetValue(i) + "\t");
                                }
                                Console.WriteLine();
                            }
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
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}

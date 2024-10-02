﻿using DataBaseApp.Connections;
using DateBaseSQL.Methods;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;

public static class Program
{
    static void Main(string[] args)
    {

        File.WriteAllText("appsettings.json", "{\n  \"ConnectionString\": {\n    \"PgConnection\": \"\"\n  }\n}");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Host (localhost): ");
        string host = Console.ReadLine();

        Console.Write("Port (5432): ");
        string port = Console.ReadLine();

        Console.Write("Database nomi (Name): ");
        string database = Console.ReadLine();

        Console.Write("User ID (postgres): ");
        string userId = Console.ReadLine();

        Console.Write("Password: ");
        //string password = Console.ReadLine();

        string password = string.Empty;
        ConsoleKeyInfo keyInfo;
        do
        {
            keyInfo = Console.ReadKey(true);

            if (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Backspace)
            {
                password += keyInfo.KeyChar;
                Console.Write("*");
            }
            else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
        }

        while (keyInfo.Key != ConsoleKey.Enter);


        Console.Clear();

        string connectionString = $"Host={host};Port={port};Database={database};User Id={userId};Password={password};";

        var appSettings = new
        {
            ConnectionString = new
            {
                PgConnection = connectionString
            }
        };

        string json = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
        File.WriteAllText("appsettings.json", json);

        var loadedSettings = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText("appsettings.json"));
        string pgConnection = loadedSettings.ConnectionString.PgConnection;


        bool exit = false;
        int selectedIndex = 0;
        Console.WriteLine("          Sechmas");
        List<string> Buyruqlar = new List<string>
        {
            "Create Table",
            "List Table",
            "Alter Table",
            "Delete Table",
            "Working with Elements",
            "Insert into Column"
        };

        while (!exit)
        {
            Console.Clear();
            for (int i = 0; i < Buyruqlar.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine(Buyruqlar[i]);
                Console.ResetColor();
            }

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1) % Buyruqlar.Count;
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex - 1 + Buyruqlar.Count) % Buyruqlar.Count;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                switch (selectedIndex)
                {
                    case 0:
                        Create.CreateTable(connectionString);
                        break;
                    case 1:
                        Select.GetTableNames(connectionString);
                        break;
                    case 2:
                        Alter.AlterTable(connectionString);
                        break;
                    case 3:
                        Deleting.DeleteTable(connectionString);
                        break;
                    case 4:
                        Class1.Elements(connectionString);
                        break;
                    case 5: 
                        Insert.InsertIntoColumn(connectionString);
                        break;
                    default:
                        Console.WriteLine("Error!!!");
                        break;

                }

                Console.ReadKey();

            }
        }
        Console.Clear();
    }
}
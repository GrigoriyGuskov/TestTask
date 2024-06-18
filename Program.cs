using Microsoft.Data.SqlClient;
using System;
using System.Data;

class Program
{
    static void Main()
    {
        SqlConnection connection = EmployeesDBinfo.Connect();

        PrintCommands();
        string text;
        do
        {
            Console.WriteLine("\nВведите команду: ");
            text = GetString();
            switch (text)
            {
                case "exit":
                    break;
                case "help":
                    PrintCommands();
                    break;
                case "reconnect":
                    connection = EmployeesDBinfo.ReConnect();
                    break;
                case "add":
                    EmployeesDBinfo.AddEmployee();
                    break;
                case "print":
                    EmployeesDBinfo.PrintAll();
                    break;
                case "update":
                    EmployeesDBinfo.ModifyEmployee(EmployeesDBinfo.Mods.update);
                    break;
                case "delete":
                    EmployeesDBinfo.ModifyEmployee(EmployeesDBinfo.Mods.delete);
                    break;
                default:
                    Console.WriteLine($"Неизвестная команда \"{text}\". " +
                        "Введите \"Help\", чтобы посмотреть список всех доступных команд.");
                    break;
            }
        } while (text != "exit");


        if (connection != null)
            connection.Close();
    }

    static void PrintCommands()
    {
        Console.WriteLine("Список доступных команд:");
        Console.WriteLine("Help - просмотр списка доступных команд");
        Console.WriteLine("Reconnect - изменение данных БД и переподключение");
        Console.WriteLine("Add - добавление нового сотрудника");
        Console.WriteLine("Print - просмотр всех сотрудников");
        Console.WriteLine("Update - обновление информации о сотруднике");
        Console.WriteLine("Delete - удаление сотрудника");
        Console.WriteLine("Exit - выход из приложения\n");
    }

    public static string GetString()
    {
        return Console.ReadLine().ToLower().Trim(new char[] {' ', '\t', '\v'});
    }
}

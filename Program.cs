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
            text = Console.ReadLine();
            text.ToLower();
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
                    var worker = new Employee();
                    if (worker.SetEmployee())
                        EmployeesDBinfo.AddEmployee(worker);
                    break;
                case "print":
                    EmployeesDBinfo.PrintAll();
                    break;
                case "update":
                    EmployeesDBinfo.UpdateEmployee();
                    break;
                case "delete":
                    EmployeesDBinfo.DeleteEmployee();
                    break;
                case "deleteall":
                    EmployeesDBinfo.DeleteAllEmployees();
                    break;
                default:
                    Console.WriteLine("Неизвестная команда. Введите \"Help\", чтобы посмотреть список всех доступных команд.");
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
        Console.WriteLine("DeleteAll - удаление данных всех сотрудников");
        Console.WriteLine("Exit - выход из приложения\n");
    }
}

using Microsoft.Data.SqlClient;
using System;
using System.Data;

class Program
{
    static void Main()
    {
        SqlConnection connection = EmployeesDBinfo.Connect();

        PrintCommands();

        Console.WriteLine("Введите команду: ");
        string text = Console.ReadLine();
        text.ToLower();

        while(text != "exit")
        {
            switch(text)
            {
                case "help":
                    PrintCommands();
                    break;
                case "reconnect":
                    connection = EmployeesDBinfo.ReConnect();
                    break;
                case "add":
                    var worker = new Employee();
                    if(worker.SetEmployee())
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
                    Console.WriteLine("Неизвестная команда");
                    break;
            }

            Console.WriteLine("\nВведите команду: ");
            text = Console.ReadLine();
            text.ToLower();
        }
        
        if (connection != null)
            connection.Close();
    }
    static void PrintCommands()
    {
        Console.WriteLine("Список доступных команд:");
        Console.WriteLine("Help - посмотреть список доступных команд");
        Console.WriteLine("Reconnect - изменить данные БД и переподключиться");
        Console.WriteLine("Add - добавление нового сотрудника");
        Console.WriteLine("Print - просмотр всех сотрудников");
        Console.WriteLine("Update - обновление информации о сотруднике");
        Console.WriteLine("Delete - удаление сотрудника");
        Console.WriteLine("DeleteAll - удаление данных всех сотрудников");
        Console.WriteLine("Exit - выход из приложения\n");
    }
}

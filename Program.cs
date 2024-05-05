using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        SqlConnection connection = EmployeesDBinfo.Connect();

        Console.WriteLine("Список доступных команд:");
        Console.WriteLine("Add - добавление нового сотрудника");
        Console.WriteLine("Print - просмотр всех сотрудников");
        Console.WriteLine("Update - обновление информации о сотруднике");
        Console.WriteLine("Delete - удаление сотрудника");
        Console.WriteLine("Exit - выход из приложения\n");

        Console.WriteLine("Введите команду: ");
        string text = Console.ReadLine();
        text.ToLower();

        while(text != "exit")
        {
            switch(text)
            {
                case "add":
                    var worker = new Employee();
                    if(worker.SetEmployee())
                        EmployeesDBinfo.AddEmployee(worker);
                    break;
                case "print":
                    EmployeesDBinfo.PrintAll();
                    break;
                case "update":
                    break;
                case "delete":
                    EmployeesDBinfo.DeleteEmployee();
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
}

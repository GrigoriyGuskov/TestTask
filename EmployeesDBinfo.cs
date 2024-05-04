using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;




internal static class EmployeesDBinfo
{
    private static string _server = "DESKTOP-VGDRUF3";
    private static string _database = "EmployeeDB";
    private static SqlConnection _connection = null;

    public static SqlConnection Connection 
    {
        get 
        {
            return _connection; 
        } 
    }
    public static string ConnectionString
    {
        get
        {
            return "Server=" + _server + ";Database=" + _database + ";Trusted_Connection=True;Trust Server Certificate=True";
        }
    }

    public static SqlConnection Connect()
    {
        _connection = new SqlConnection(EmployeesDBinfo.ConnectionString);
        try
        {
            _connection.Open();
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
            _connection = null;
        }   
        finally
        {
            if (_connection != null)
            {
                // Вывод информации о подключении
                Console.WriteLine("Свойства подключения:");
                Console.WriteLine($"\tСтрока подключения: {_connection.ConnectionString}");
                Console.WriteLine($"\tБаза данных: {_connection.Database}");
                Console.WriteLine($"\tСервер: {_connection.DataSource}");
                Console.WriteLine($"\tВерсия сервера: {_connection.ServerVersion}");
                Console.WriteLine($"\tСостояние: {_connection.State}");
                Console.WriteLine($"\tWorkstationld: {_connection.WorkstationId}");
            }  
        }
        
        return _connection;
    }

    public static void AddEmployee(Employee worker)
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            Console.WriteLine("Отсутствует подключение");
            return;
        }
        if (!worker.IsValid())
        {
            Console.WriteLine("Неверные данные работника");
            return;
        }
        string commandString = $"INSERT INTO Employees (FirstName, LastName, Email, DateOfBirth, Salary) VALUES ('{worker.FirstName}', '{worker.LastName}', '{worker.Email}', '{worker.DateOfBirth.Year}.{worker.DateOfBirth.Month}.{worker.DateOfBirth.Day}', {worker.Salary})";
        SqlCommand command = new SqlCommand(commandString, _connection);
        var res =  command.ExecuteNonQuery();
        if (res == 0)
            Console.WriteLine($"Не удалось добавить работника {worker.FirstName} {worker.LastName} в базу данных");
    }
    public static void PrintAll()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            Console.WriteLine("Отсутствует подключение");
            return;
        }
        string commandString = "SELECT * FROM Employees";
        SqlCommand command = new SqlCommand(commandString, _connection);
        SqlDataReader reader = command.ExecuteReader();
        if (reader.HasRows) 
        {
            Console.WriteLine($"{reader.GetName(0),25}{reader.GetName(1),25}{reader.GetName(2),25}{reader.GetName(3),25}{reader.GetName(4),25}{reader.GetName(5),25}");
            while (reader.Read()) 
            {
                Console.WriteLine($"{reader.GetValue(0),25}{reader.GetValue(1),25}{reader.GetValue(2),25}{reader.GetValue(3),25}{reader.GetValue(4).ToString().Substring(0,10),25}{reader.GetValue(5),25}");
            }
        }
        reader.Close();
    }
}


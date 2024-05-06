using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;




internal static class EmployeesDBinfo
{
    private static string _server = "DESKTOP-VGDRUF3";
    private static string _database = "EmployeeDB";
    private static string _table = "Employees";
    private static SqlConnection _connection = null;
    public static string Table { get { return _table; } }
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
        }
        finally
        {
            
            // Вывод информации о подключении
            Console.WriteLine("Свойства подключения:");
            Console.WriteLine($"\tБаза данных: {_connection.Database}");
            Console.WriteLine($"\tСервер: {_connection.DataSource}");
            Console.WriteLine($"\tСостояние: {_connection.State}");
            if(_connection.State != ConnectionState.Open)
            {
                _connection = null;
            }

        }
        
        return _connection;
    }
    public static SqlConnection ReConnect()
    {
        Console.WriteLine("Введите новое имя сервера или \"-\" чтобы оставить без изменений или \"Cancel\" для отмены:");
        string text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return _connection;
        if(text != "-")
            _server = text;
        Console.WriteLine("Введите новое название базы данных или \"-\" чтобы оставить без изменений или \"Cancel\" для отмены:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return _connection;
        if (text != "-")
            _database = text;
        return Connect();
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
        string commandString = $"INSERT INTO {_table} (FirstName, LastName, Email, DateOfBirth, Salary) VALUES ('{worker.FirstName}', '{worker.LastName}', '{worker.Email}', '{worker.DateOfBirth.Year}.{worker.DateOfBirth.Month}.{worker.DateOfBirth.Day}', {worker.Salary})";
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
        string commandString = $"SELECT * FROM {_table}";
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
    public static void DeleteEmployee()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            Console.WriteLine("Отсутствует подключение");
            return;
        }
        Console.WriteLine("Введите ID сотрудника");
        string text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return;
        
        while (true)
        {
            string commandString = $"SELECT * FROM {_table} WHERE EmployeeID = {text}";
            SqlCommand command = new SqlCommand(commandString, _connection);
            SqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine($"Не удалось найти сотрудника с ID {text}. Попробуйте еще раз или введите \"Cancel\" для отмены");
                text = Console.ReadLine();
                if (text.ToLower() == "cancel")
                {
                    reader.Close();
                    return;
                }
            }
            else
            {
                reader.Close();
                commandString = $"DELETE FROM {_table} WHERE EmployeeID = {text}";
                command = new SqlCommand(commandString, _connection);
                command.ExecuteNonQuery();
                return;
            }
        }

    }

    public static void UpdateEmployee()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            Console.WriteLine("Отсутствует подключение");
            return;
        }
        Console.WriteLine("Введите ID сотрудника");
        string text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return;

        while (true)
        {
            string commandString = $"SELECT * FROM {_table} WHERE EmployeeID = {text}";
            SqlCommand command = new SqlCommand(commandString, _connection);
            SqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine($"Не удалось найти сотрудника с ID {text}. Попробуйте еще раз или введите \"Cancel\" для отмены");
                text = Console.ReadLine();
                if (text.ToLower() == "cancel")
                {
                    reader.Close();
                    return;
                }
            }
            else
            {
                reader.Close();
                commandString = Employee.GetUpdateString();
                if (commandString == null)
                    return;
                commandString += $" WHERE EmployeeID = {text}";
                command = new SqlCommand(commandString, _connection);
                command.ExecuteNonQuery();
                return;
            }
        }

    }

}


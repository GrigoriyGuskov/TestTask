using Microsoft.Data.SqlClient;
using System;
using System.Data;




internal static class EmployeesDBinfo
{
    private static string _server = "DESKTOP-VGDRUF3";
    private static string _database = "EmployeeDB";
    private static string _table = "Employees";
    private static SqlConnection _connection = null;
    private static string[] _columns = {    "EmployeeID",
                                            "FirstName",
                                            "LastName",
                                            "Email",
                                            "DateOfBirth",
                                            "Salary"
                                        };
    public static string Table { get { return _table; } }
    public static string[] Columns { get { return _columns; } }
    public static string ConnectionString
    {
        get
        {
            return "Server=" + _server + ";Database=" + _database + ";Trusted_Connection=True;Trust Server Certificate=True";
        }
    }

    public static SqlConnection Connect()
    {
        _connection = new SqlConnection(ConnectionString);
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
            else
            {
                CheckTable(_table);
            }

        }
        
        return _connection;
    }

    public static bool CheckTable(string tableName)
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            Console.WriteLine("Отсутствует подключение");
            return false;
        }
        string commandString = $"SELECT COUNT(*) FROM sys.tables WHERE name='{tableName}'";
        SqlCommand command = new SqlCommand(commandString, _connection);
        int res = (int)command.ExecuteScalar();
        if (res != 1)
        {
            Console.WriteLine($"Отсутствует таблица \"{tableName}\"");
            return false;
        }
        return true;
    } 
    public static bool CheckColumns(string tableName, string[] columnNames)
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            Console.WriteLine("Отсутствует подключение");
            return false;
        }
        if (!CheckTable(_table))
            return false;
        bool cr = true;
        foreach (string column in columnNames)
        {
            string commandString = $"SELECT COUNT(*) FROM information_schema.COLUMNS WHERE TABLE_NAME='{tableName}' AND COLUMN_NAME='{column}'";
            SqlCommand command = new SqlCommand(commandString, _connection);
            int res = (int)command.ExecuteScalar();
            if (res != 1)
            {
                Console.WriteLine($"Отсутствует столбец \"{column}\" в таблице \"{tableName}\"");
                cr = false;
            }
        }
        return cr;
    }
    public static SqlConnection ReConnect()
    {
        Console.WriteLine("Введите новое имя сервера или \"-\" чтобы оставить без изменений или \"Cancel\" для отмены:");
        string text = Console.ReadLine();
        if (text.Contains(';'))
        {
            Console.WriteLine("Вероятная попытка SQL-инъекции");
            return _connection;
        }
        if (text.ToLower() == "cancel")
            return _connection;
        if(text != "-")
            _server = text;
        Console.WriteLine("Введите новое название базы данных или \"-\" чтобы оставить без изменений или \"Cancel\" для отмены:");
        text = Console.ReadLine();
        if (text.Contains(';'))
        {
            Console.WriteLine("Вероятная попытка SQL-инъекции");
            return _connection;
        }
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
        if (!CheckTable(_table) || !CheckColumns(_table, _columns))
            return;
        if (!worker.IsValid())
        {
            Console.WriteLine("Неверные данные работника");
            return;
        }
        string commandString = $"INSERT INTO {_table} ({_columns[1]}, {_columns[2]}, {_columns[3]}, {_columns[4]}, {_columns[5]}) VALUES ('{worker.FirstName}', '{worker.LastName}', '{worker.Email}', '{worker.DateOfBirth.Year}.{worker.DateOfBirth.Month}.{worker.DateOfBirth.Day}', {worker.Salary})";
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
        if (!CheckTable(_table))
            return;
        string commandString = $"SELECT * FROM {_table}";
        SqlCommand command = new SqlCommand(commandString, _connection);
        SqlDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            Console.WriteLine($"{reader.GetName(0),25}{reader.GetName(1),25}{reader.GetName(2),25}{reader.GetName(3),25}{reader.GetName(4),25}{reader.GetName(5),25}");
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetValue(0),25}{reader.GetValue(1),25}{reader.GetValue(2),25}{reader.GetValue(3),25}{reader.GetValue(4).ToString().Substring(0, 10),25}{reader.GetValue(5),25}");
            }
        }
        else
            Console.WriteLine("В таблице отсутствуют данные");
        reader.Close();
    }
    public static void DeleteEmployee()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            Console.WriteLine("Отсутствует подключение");
            return;
        }
        if (!CheckTable(_table) || !CheckColumns(_table, new string[] { _columns[0]}))
            return;
        Console.WriteLine("Введите ID сотрудника");
        string text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return;
        int ID;
        while (true)
        {
            if (!int.TryParse(text, out ID))
            {
                Console.WriteLine($"\"{text}\" не является целым числом. Попробуйте еще раз или введите \"Cancel\" для отмены");
                text = Console.ReadLine();
                if (text.ToLower() == "cancel")
                    return;
            }
            else
            {
                string commandString = $"SELECT * FROM {_table} WHERE {_columns[0]} = {ID}";
                SqlCommand command = new SqlCommand(commandString, _connection);
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    Console.WriteLine($"Не удалось найти сотрудника с ID {ID}. Попробуйте еще раз или введите \"Cancel\" для отмены");
                    text = Console.ReadLine();
                    if (text.ToLower() == "cancel")
                    {
                        reader.Close();
                        return;
                    }
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    commandString = $"DELETE FROM {_table} WHERE {_columns[0]} = {ID}";
                    command = new SqlCommand(commandString, _connection);
                    command.ExecuteNonQuery();
                    return;
                }
            }
        }

    }
    public static void DeleteAllEmployees()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            Console.WriteLine("Отсутствует подключение");
            return;
        }
        if (!CheckTable(_table))
            return;
        Console.WriteLine("Вы уверены, что хотите удалить все данные?");
        Console.WriteLine("Введите \"Yes\" или \"No\":");
        string text = Console.ReadLine();
        text.ToLower();
        while (text != "yes")
        {
            if (text == "no")
                return;
            Console.WriteLine("Я не понимаю. Попробуйте еще раз");
            text = Console.ReadLine().ToLower();
        }
        string commandString = $"TRUNCATE TABLE {_table}";
        SqlCommand command = new SqlCommand(commandString, _connection);
        command.ExecuteNonQuery();
    }

    public static void UpdateEmployee()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            Console.WriteLine("Отсутствует подключение");
            return;
        }
        if (!CheckTable(_table) || !CheckColumns(_table, new string[] {_columns[0]}))
            return;
        Console.WriteLine("Введите ID сотрудника");
        string text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return;
        int ID;
        while (true)
        {
            if (!int.TryParse(text, out ID))
            {
                Console.WriteLine($"\"{text}\" не является целым числом. Попробуйте еще раз или введите \"Cancel\" для отмены");
                text = Console.ReadLine();
                if (text.ToLower() == "cancel")
                    return;
            }
            else
            {
                string commandString = $"SELECT * FROM {_table} WHERE {_columns[0]} = {ID}";
                SqlCommand command = new SqlCommand(commandString, _connection);
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    Console.WriteLine($"Не удалось найти сотрудника с ID {ID}. Попробуйте еще раз или введите \"Cancel\" для отмены");
                    text = Console.ReadLine();
                    if (text.ToLower() == "cancel")
                    {
                        reader.Close();
                        return;
                    }
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    commandString = Employee.GetUpdateString();
                    if (commandString == null)
                        return;
                    commandString += $" WHERE {_columns[0]} = {ID}";
                    command = new SqlCommand(commandString, _connection);
                    command.ExecuteNonQuery();
                    return;
                }
            }
        }
    }

}


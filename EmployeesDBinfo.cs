using Microsoft.Data.SqlClient;
using System;
using System.Data;



internal static class EmployeesDBinfo
{
    public const string CANCEL = "Cancel";
    public const string LEAVE = "-";
    public const string TRY_AGAIN_STRING = "Попробуйте еще раз или введите \"" + CANCEL + "\" для отмены";
    public const string LEAVE_STRING = "\"" + LEAVE + "\" чтобы оставить без изменений или \"" + CANCEL + "\" для отмены:";

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
    public enum Mods 
    { 
        delete,
        update,
    }
    public static string Table { get { return _table; } }
    public static string[] Columns { get { return _columns; } }
    public static string ConnectionString
    {
        get
        {
            return "Server=" + _server + ";Database=" + _database + ";" +
                "Trusted_Connection=True;Trust Server Certificate=True";
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
            return null;
        }
            
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

        return _connection;
    }

    public static bool CheckTable(string tableName)
    {
        if (!CheckConnection())
        {
            return false;
        }
        string commandString = $"SELECT case when EXISTS(SELECT 1 FROM sys.tables " +
            $"WHERE name='{tableName}') then 1 else 0 end";
        SqlCommand command = new SqlCommand(commandString, _connection);
        int res;
        try
        {
            res = (int)command.ExecuteScalar();
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        return true;
        
    } 
    public static bool CheckColumns(string tableName, string[] columnNames)
    {
        if (!CheckConnection())
        {
            return false;
        }
        if (!CheckTable(_table))
            return false;
        foreach (string column in columnNames)
        {
            string commandString = $"SELECT case when EXISTS(SELECT 1 FROM information_schema.COLUMNS " +
                $"WHERE TABLE_NAME='{tableName}' AND COLUMN_NAME='{column}') then 1 else 0 end";
            SqlCommand command = new SqlCommand(commandString, _connection);
            int res;
            try 
            { 
                res = (int)command.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        return true;
    }
    public static bool CheckConnection()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            Console.WriteLine("Отсутствует подключение");
            return false;
        }
        return true;
    }
    public static SqlConnection ReConnect()
    {
        Console.WriteLine("Введите новое имя сервера или " + LEAVE_STRING);
        string text = Program.GetString();
        if (text.Contains(';'))
        {
            Console.WriteLine("Вероятная попытка SQL-инъекции");
            return _connection;
        }
        if (text == CANCEL)
            return _connection;
        if(text != LEAVE)
            _server = text;
        Console.WriteLine("Введите новое название базы данных или " + LEAVE_STRING);
        text = Program.GetString();
        if (text.Contains(';'))
        {
            Console.WriteLine("Вероятная попытка SQL-инъекции");
            return _connection;
        }
        if (text == CANCEL)
            return _connection;
        if (text != LEAVE)
            _database = text;
        return Connect();
    }

    public static void AddEmployee()
    {
        if (!CheckConnection())
        {
            return;
        }
        if (!CheckTable(_table) || !CheckColumns(_table, _columns))
            return;
        var worker = new Employee();
        if (!worker.SetEmployee())
            return;
        
        string commandString = "Add";
        SqlCommand command = new SqlCommand(commandString, _connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@FirstName", worker.FirstName);
        command.Parameters.AddWithValue("@LastName", worker.LastName);
        command.Parameters.AddWithValue("@Email", worker.Email);
        command.Parameters.AddWithValue("@DateOfBirth", worker.DateOfBirth);
        command.Parameters.AddWithValue("@Salary", worker.Salary);
        int res = 0;
        try
        {
            res = command.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
        if (res == 0)
            Console.WriteLine($"Не удалось добавить работника {worker.FirstName} {worker.LastName} в базу данных");
    }
    public static void PrintAll()
    {
        if (!CheckConnection())
        {
            return;
        }
        if (!CheckTable(_table))
            return;
        string commandString = "Print";
        SqlCommand command = new SqlCommand(commandString, _connection);
        command.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader;
        try
        {
            reader = command.ExecuteReader();
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
        if (reader.HasRows)
        {
            for (int i = 0; i < reader.FieldCount; ++i)
                Console.Write($"{reader.GetName(i),25}");
            Console.Write("\n");
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; ++i)
                    if(i == reader.GetOrdinal(_columns[4]))
                        Console.Write($"{DateOnly.FromDateTime((DateTime)reader.GetValue(i)),25}");
                    else
                        Console.Write($"{reader.GetValue(i),25}");
                Console.Write("\n");
            }
        }
        else
            Console.WriteLine("В таблице отсутствуют данные");
        reader.Close();
    }
    

    public static void ModifyEmployee(Mods mod)
    {
        if (!CheckConnection())
        {
            return;
        }
        if (!CheckTable(_table) || !CheckColumns(_table, new string[] {_columns[0]}))
            return;
        Console.WriteLine("Введите ID сотрудника");
        string text = Program.GetString();
        if (text == CANCEL)
            return;
        int ID;
        while (true)
        {
            if (!int.TryParse(text, out ID))
            {
                Console.WriteLine($"\"{text}\" не является целым числом. " + TRY_AGAIN_STRING);
                text = Program.GetString();
                if (text == CANCEL)
                    return;
            }
            else
            {
                string commandString = "GetEmp";
                SqlCommand command = new SqlCommand(commandString, _connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmpId", ID);
                SqlDataReader reader;
                try
                {
                    reader = command.ExecuteReader();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
                if (!reader.HasRows)
                {
                    Console.WriteLine($"Не удалось найти сотрудника с ID {ID}. " + TRY_AGAIN_STRING);
                    text = Program.GetString();
                    if (text == CANCEL)
                    {
                        reader.Close();
                        return;
                    }
                    reader.Close();
                }
                else
                {
                    
                    switch (mod)
                    {
                        case Mods.update:
                            Employee worker = new Employee();
                            if (!worker.SetEmployee(reader) || !worker.UpdateEmployee())
                            {
                                reader.Close();
                                return;
                            }
                            commandString = "Update";
                            command = new SqlCommand(commandString, _connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@EmpId", ID);
                            command.Parameters.AddWithValue("@FirstName", worker.FirstName);
                            command.Parameters.AddWithValue("@LastName", worker.LastName);
                            command.Parameters.AddWithValue("@Email", worker.Email);
                            command.Parameters.AddWithValue("@DateOfBirth", worker.DateOfBirth);
                            command.Parameters.AddWithValue("@Salary", worker.Salary);
                            break;
                        case Mods.delete:
                            commandString = "Delete";
                            command = new SqlCommand(commandString, _connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@EmpId", ID);
                            break;
                    }
                    reader.Close();
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return;
                    }
                    return;
                }
            }
        }
    }

}


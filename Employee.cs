using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;


internal class Employee
{

    private string _firstName;
    private string _lastName;
    private string _email;
    private DateOnly _dateOfBirth;
    private decimal _salary;

    public string FirstName
    {
        get
        {
            return _firstName;
        }
        set
        {
            _firstName = value;
        }
    }
    public string LastName
    {
        get
        {
            return _lastName;
        }
        set
        {
            _lastName = value;
        }
    }
    public string Email
    {
        get
        {
            return _email;
        }
        set
        {
            _email = value;
        }
    }
    public DateOnly DateOfBirth
    {
        get
        {
            return _dateOfBirth;
        }
        set
        {
            _dateOfBirth = value;
        }
    }
    public decimal Salary
    {
        get
        {
            return _salary;
        }
        set
        {
            _salary = value;
        }
    }

    public bool SetEmployee()
    {

        Console.WriteLine("Введите имя:");
        string text = Console.ReadLine().Trim();
        if (text.ToLower() == EmployeesDBinfo.CANCEL)
            return false;

        while (!IsValidName(text))
        {
            Console.WriteLine(EmployeesDBinfo.TRY_AGAIN_STRING);
            text = Console.ReadLine().Trim();
            if (text.ToLower() == EmployeesDBinfo.CANCEL)
                return false;
        }
        FirstName = text;

        Console.WriteLine("Введите фамилию:");
        text = Console.ReadLine().Trim();
        if (text.ToLower() == EmployeesDBinfo.CANCEL)
            return false;
        
        while (!IsValidName(text))
        {
            Console.WriteLine(EmployeesDBinfo.TRY_AGAIN_STRING);
            text = Console.ReadLine().Trim();
            if (text.ToLower() == EmployeesDBinfo.CANCEL)
                return false;
        }
        LastName = text;

        Console.WriteLine("Введите email:");
        text = Console.ReadLine().Trim();
        if (text.ToLower() == EmployeesDBinfo.CANCEL)
            return false;
        
        while (!IsValidEmail(text))
        {
            Console.WriteLine(EmployeesDBinfo.TRY_AGAIN_STRING);
            text = Console.ReadLine().Trim();
            if (text.ToLower() == EmployeesDBinfo.CANCEL)
                return false;
        }
        Email = text;

        Console.WriteLine("Введите дату рождения в формате DD.MM.YYYY:");
        text = Program.GetString();
        if (text.ToLower() == EmployeesDBinfo.CANCEL)
            return false;
        DateOnly resDOB;
        while (!DateOnly.TryParse(text,out resDOB) || !IsValidDateOfBirth(resDOB))
        {
            Console.WriteLine("Проверьте корректность введённой даты. " + EmployeesDBinfo.TRY_AGAIN_STRING);
            text = Program.GetString();
            if (text.ToLower() == EmployeesDBinfo.CANCEL)
                return false;
        }
        DateOfBirth = resDOB;

        Console.WriteLine("Введите зарплату:");
        text = Program.GetString();
        if (text.ToLower() == EmployeesDBinfo.CANCEL)
            return false;
        decimal resS;
        while (!decimal.TryParse(text, out resS) || !IsValidSalary(resS))
        {
            Console.WriteLine("Видимо в бухгалтерии опять всё перепутали. " + EmployeesDBinfo.TRY_AGAIN_STRING);
            text = Program.GetString();
            if (text.ToLower() == EmployeesDBinfo.CANCEL)
                return false;
        }
        Salary = resS;

        return true;
    }

    public bool SetEmployee(SqlDataReader reader)
    {
        if (!reader.HasRows)
        {
            return false;

        }
        reader.Read();

        string text = (string)reader.GetValue(1);
        if (!IsValidName(text))
        {
                return false;
        }
        FirstName = text;

        
        text = (string)reader.GetValue(2);
        if (!IsValidName(text))
        {
            return false;
        }
        LastName = text;

        text = (string)reader.GetValue(3);
        if (!IsValidEmail(text))
        {
            return false;
        }
        Email = text;

        DateOnly date = DateOnly.FromDateTime((DateTime)reader.GetValue(4));
        if (!IsValidDateOfBirth(date))
        {
            return false;
        }
        DateOfBirth = date;

        
        decimal salary = (decimal)reader.GetValue(5); ;
        if (!IsValidSalary(salary))
        {
            return false;
        }
        Salary = salary;

        return true;
    }

    public bool UpdateEmployee()
    {

        bool updfl = false;

        Console.WriteLine("Введите новое имя или " + EmployeesDBinfo.LEAVE_STRING);
        string text = Console.ReadLine().Trim();
        if (text.ToLower() == EmployeesDBinfo.CANCEL)
            return false;

        while (text != EmployeesDBinfo.LEAVE && !Employee.IsValidName(text))
        {
            Console.WriteLine(EmployeesDBinfo.TRY_AGAIN_STRING);
            text = Console.ReadLine().Trim();
            if (text.ToLower() == EmployeesDBinfo.CANCEL)
                return false;
        }
        if (text != EmployeesDBinfo.LEAVE)
        {
            updfl = true;
            FirstName = text;
        }

        Console.WriteLine("Введите новую фамилию или " + EmployeesDBinfo.LEAVE_STRING);
        text = Console.ReadLine().Trim();
        if (text.ToLower() == EmployeesDBinfo.CANCEL)
            return false;

        while (text != EmployeesDBinfo.LEAVE && !Employee.IsValidName(text))
        {
            Console.WriteLine(EmployeesDBinfo.TRY_AGAIN_STRING);
            text = Console.ReadLine().Trim();
            if (text.ToLower() == EmployeesDBinfo.CANCEL)
                return false;
        }
        if (text != EmployeesDBinfo.LEAVE)
        {
            updfl = true;
            LastName = text;
        }

        Console.WriteLine("Введите новый email или " + EmployeesDBinfo.LEAVE_STRING);
        text = Program.GetString();
        if (text.ToLower() == EmployeesDBinfo.CANCEL)
            return false;

        while (text != EmployeesDBinfo.LEAVE && !Employee.IsValidEmail(text))
        {
            Console.WriteLine(EmployeesDBinfo.TRY_AGAIN_STRING);
            text = Program.GetString();
            if (text.ToLower() == EmployeesDBinfo.CANCEL)
                return false;
        }
        if (text != EmployeesDBinfo.LEAVE)
        {
            updfl = true;
            Email = text;
        }

        Console.WriteLine("Введите новую дату рождения в формате DD.MM.YYYY или " + EmployeesDBinfo.LEAVE_STRING);
        text = Program.GetString();
        if (text.ToLower() == EmployeesDBinfo.CANCEL)
            return false;
        DateOnly resDOB = DateOnly.FromDateTime(DateTime.Now);
        while (text != EmployeesDBinfo.LEAVE && (!DateOnly.TryParse(text, out resDOB) || !Employee.IsValidDateOfBirth(resDOB)))
        {
            Console.WriteLine("Проверьте корректность введённой даты. " + EmployeesDBinfo.TRY_AGAIN_STRING);
            text = Program.GetString();
            if (text.ToLower() == EmployeesDBinfo.CANCEL)
                return false;
        }
        if (text != EmployeesDBinfo.LEAVE)
        {
            updfl = true;
            DateOfBirth = resDOB;

        }

        Console.WriteLine("Введите новую зарплату или " + EmployeesDBinfo.LEAVE_STRING);
        text = Program.GetString();
        if (text.ToLower() == EmployeesDBinfo.CANCEL)
            return false;
        decimal resS = -1;
        while (text != EmployeesDBinfo.LEAVE && (!decimal.TryParse(text, out resS) || !Employee.IsValidSalary(resS)))
        {
            Console.WriteLine("Видимо в бухгалтерии опять всё перепутали. " + EmployeesDBinfo.TRY_AGAIN_STRING);
            text = Program.GetString();
            if (text.ToLower() == EmployeesDBinfo.CANCEL)
                return false;
        }
        if (text != EmployeesDBinfo.LEAVE)
        {
            updfl = true;
            Salary = resS;
        }
        
        return updfl;
    }
    public bool IsValid()
    {
        return IsValidName(FirstName) && IsValidName(LastName) && IsValidEmail(Email) && IsValidDateOfBirth(DateOfBirth) && IsValidSalary(Salary);
    }

    public static bool IsValidName(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Имя или фамилия не может быть пустым");
            return false;
        }
        if(name.Length > 50)
        {
            Console.WriteLine("Слишком длинное имя или фамилия");
            return false;
        }
        foreach (char let in name)
            if (!char.IsLetter(let))
            {
                Console.WriteLine("Имя или фамилия могут содержать только буквы");
                return false;
            }
        return true;
    }
    public static bool IsValidEmail(string email)
    {
        if (email.IsNullOrEmpty())
        {
            return false;
        }
        return new EmailAddressAttribute().IsValid(email);
    }

    public static bool IsValidDateOfBirth(DateOnly dateOfBirth)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        if (today < dateOfBirth.AddYears(14))
        {
            Console.WriteLine("Возраст не должен быть меньше 14 лет");
            return false;
        }
        return true;
    }
    public static bool IsValidSalary(decimal salary)
    {
        if (salary < 0)
        {
            Console.WriteLine("Зарплата не может быть отрицательной");
            return false;
        }
        if(salary > (decimal)9999999999999999.99)
        {
            Console.WriteLine("Слишком большая зарплата");
            return false;
        }
        return true;
    }


}


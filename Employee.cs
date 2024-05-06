using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
        string text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return false;

        while (!IsValidName(text))
        {
            Console.WriteLine("Это имя не подходит. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return false;
        }
        FirstName = text;

        Console.WriteLine("Введите фамилию:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return false;
        
        while (!IsValidName(text))
        {
            Console.WriteLine("Эта фамилия не подходит. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return false;
        }
        LastName = text;

        Console.WriteLine("Введите email:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return false;
        
        while (!IsValidEmail(text))
        {
            Console.WriteLine("Этот email не подходит. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return false;
        }
        Email = text;

        Console.WriteLine("Введите дату рождения в формате DD/MM/YYYY:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return false;
        DateOnly resDOB;
        while (!DateOnly.TryParse(text,out resDOB) || !IsValidDateOfBirth(resDOB))
        {
            Console.WriteLine("Проверьте корректность введённой даты, а также мы не принимаем на работу лиц младше 14 лет. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return false;
        }
        DateOfBirth = resDOB;

        Console.WriteLine("Введите зарплату:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return false;
        decimal resS;
        while (!decimal.TryParse(text, out resS) || !IsValidSalary(resS))
        {
            Console.WriteLine("Видимо в бухгалтерии опять всё перепутали. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return false;
        }
        Salary = resS;

        return true;
    }

    public static string GetUpdateString()
    {
        string commandString = $"UPDATE {EmployeesDBinfo.Table} ";

        bool setfl = false;

        Console.WriteLine("Введите новое имя или \"-\" чтобы оставить без изменений:");
        string text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return null;

        while (text != "-" && !Employee.IsValidName(text))
        {
            Console.WriteLine("Это имя не подходит. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return null;
        }
        if (text != "-")
        {
            if (!setfl)
            {
                setfl = true;
                commandString += "SET ";
            }
            else
                commandString += ", ";
            commandString += "FirstName = '" + text + "'";
        }

        Console.WriteLine("Введите новую фамилию или \"-\" чтобы оставить без изменений:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return null;

        while (text != "-" && !Employee.IsValidName(text))
        {
            Console.WriteLine("Эта фамилия не подходит. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return null;
        }
        if (text != "-")
        {
            if (!setfl)
            {
                setfl = true;
                commandString += "SET ";
            }
            else
                commandString += ", ";
            commandString += "LastName = '" + text + "'";
        }

        Console.WriteLine("Введите новый email или \"-\" чтобы оставить без изменений:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return null;

        while (text != "-" && !Employee.IsValidEmail(text))
        {
            Console.WriteLine("Этот email не подходит. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return null;
        }
        if (text != "-")
        {
            if (!setfl)
            {
                setfl = true;
                commandString += "SET ";
            }
            else
                commandString += ", ";
            commandString += "Email = '" + text + "'";
        }

        Console.WriteLine("Введите новую дату рождения в формате DD/MM/YYYY или \"-\" чтобы оставить без изменений:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return null;
        DateOnly resDOB = DateOnly.Parse("01/01/2000");
        while (text != "-" && (!DateOnly.TryParse(text, out resDOB) || !Employee.IsValidDateOfBirth(resDOB)))
        {
            Console.WriteLine("Проверьте корректность введённой даты, а также мы не принимаем на работу лиц младше 14 лет. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return null;
        }
        if (text != "-")
        {
            if (!setfl)
            {
                setfl = true;
                commandString += "SET ";
            }
            else
                commandString += ", ";
            commandString += $"DateOfBirth = '{resDOB.Year}.{resDOB.Month}.{resDOB.Day}'";

        }

        Console.WriteLine("Введите новую зарплату или \"-\" чтобы оставить без изменений:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return null;
        decimal resS;
        while (text != "-" && (!decimal.TryParse(text, out resS) || !Employee.IsValidSalary(resS)))
        {
            Console.WriteLine("Видимо в бухгалтерии опять всё перепутали. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return null;
        }
        if (text != "-")
        {
            if (!setfl)
            {
                setfl = true;
                commandString += "SET ";
            }
            else
                commandString += ", ";
            commandString += "Salary = " + text;
        }

        return commandString;
    }
    public bool IsValid()
    {
        return IsValidName(FirstName) && IsValidName(LastName) && IsValidEmail(Email) && IsValidDateOfBirth(DateOfBirth) && IsValidSalary(Salary);
    }

    public static bool IsValidName(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name) || name.Length > 50 || name.Split(" ").Length != 1)
            return false;
        foreach (char let in name)
            if (!char.IsLetter(let))
                return false;
        return true;
    }
    public static bool IsValidEmail(string email) 
    {
        if (email.Length < 5 || email.Length > 100)
            return false;
        int fl = 0;
        if (!char.IsLetter(email[0]))
            return false;
        foreach (char let in email)
        {
            switch (fl)
            {
                case 0:
                    if (char.IsLetterOrDigit(let))
                        ++fl;
                    else
                        return false;
                    break;
                case 1:
                    if (let == '@')
                        ++fl;
                    else if (!(let == '-' || let == '_' || let == '.' || char.IsLetterOrDigit(let)))
                        return false;
                    break;
                case 2:
                    if (char.IsLetter(let))
                        ++fl;
                    else
                        return false;
                    break;
                case 3:
                    if (let == '.')
                        ++fl;
                    else if (!char.IsLetter(let))
                        return false;
                    break;
                case 4:
                    if (char.IsLetter(let))
                        ++fl;
                    break;
                case 5:
                    if (!char.IsLetter(let))
                        return false;
                    break;
            }
            
        }
        return true;
    }
    public static bool IsValidDateOfBirth(DateOnly dateOfBirth)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        if (today < dateOfBirth.AddYears(14))
            return false;
        return true;
    }
    public static bool IsValidSalary(decimal salary)
    {
        if(salary < 0 || salary > (decimal)9999999999999999.99) 
            return false;
        return true;
    }


}


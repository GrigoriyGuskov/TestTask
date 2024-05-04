using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        FirstName = text;
        while (!IsValidFirstName())
        {
            Console.WriteLine("Это имя не подходит. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return false;
            FirstName = text;
        }
        Console.WriteLine("Введите фамилию:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return false;
        LastName = text;
        while (!IsValidLastName())
        {
            Console.WriteLine("Эта фамилия не подходит. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return false;
            LastName = text;
        }
        Console.WriteLine("Введите email:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return false;
        Email = text;
        while (!IsValidEmail())
        {
            Console.WriteLine("Этот email не подходит. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return false;
            Email = text;
        }
        Console.WriteLine("Введите дату рождения:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return false;
        DateOfBirth = DateOnly.Parse(text);
        while (!IsValidDateOfBirth())
        {
            Console.WriteLine("Мы не принимаем на работу лиц младше 14 лет. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return false;
            DateOfBirth = DateOnly.Parse(text);
        }
        Console.WriteLine("Введите зарплату:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return false;
        Salary = decimal.Parse(text);
        while (!IsValidSalary())
        {
            Console.WriteLine("Видимо в бухгалтерии опять всё перепутали. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return false;
            Salary = decimal.Parse(text);
        }
        return true;
    }
    public bool IsValid()
    {
        return IsValidFirstName() && IsValidLastName() && IsValidEmail() && IsValidDateOfBirth() && IsValidSalary();
    }

    public bool IsValidFirstName()
    {
        if (string.IsNullOrEmpty(FirstName) || string.IsNullOrWhiteSpace(FirstName) || FirstName.Length > 50 || FirstName.Split(" ").Length != 1)
            return false;
        foreach (char let in FirstName)
            if (!char.IsLetter(let))
                return false;
        return true;
    }
    public bool IsValidLastName()
    {
        if (string.IsNullOrEmpty(LastName) || string.IsNullOrWhiteSpace(LastName) || LastName.Length > 50 || LastName.Split(" ").Length != 1)
            return false;
        foreach (char let in LastName)
            if (!char.IsLetter(let))
                return false;
        return true;
    }
    public bool IsValidEmail() 
    {
        if (Email.Length < 5 || Email.Length > 100)
            return false;
        int fl = 0;
        if (!char.IsLetter(Email[0]))
            return false;
        foreach (char let in Email)
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
    public bool IsValidDateOfBirth()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        if (today < DateOfBirth.AddYears(14))
            return false;
        return true;
    }
    public bool IsValidSalary()
    {
        if(Salary < 0 || Salary > (decimal)9999999999999999.99) 
            return false;
        return true;
    }


}


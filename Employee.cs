using System;


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
            Console.WriteLine("Попробуйте еще раз или введите \"Cancel\" для отмены");
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
            Console.WriteLine("Попробуйте еще раз или введите \"Cancel\" для отмены");
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
            Console.WriteLine("Попробуйте еще раз или введите \"Cancel\" для отмены");
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
            Console.WriteLine("Проверьте корректность введённой даты. Попробуйте еще раз или введите \"Cancel\" для отмены");
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
        string commandString = $"UPDATE {EmployeesDBinfo.Table} SET ";

        bool setfl = false;

        Console.WriteLine("Введите новое имя или \"-\" чтобы оставить без изменений:");
        string text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return null;

        while (text != "-" && !Employee.IsValidName(text))
        {
            Console.WriteLine("Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return null;
        }
        if (text != "-")
        {
            setfl = true;
            commandString += EmployeesDBinfo.Columns[1] + " = '" + text + "'";
        }

        Console.WriteLine("Введите новую фамилию или \"-\" чтобы оставить без изменений:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return null;

        while (text != "-" && !Employee.IsValidName(text))
        {
            Console.WriteLine("Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return null;
        }
        if (text != "-")
        {
            if (setfl)
                commandString += ", ";
            setfl = true;
            commandString += EmployeesDBinfo.Columns[2] + " = '" + text + "'";
        }

        Console.WriteLine("Введите новый email или \"-\" чтобы оставить без изменений:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return null;

        while (text != "-" && !Employee.IsValidEmail(text))
        {
            Console.WriteLine("Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return null;
        }
        if (text != "-")
        {
            if (setfl)
                commandString += ", ";
            setfl = true;
            commandString += EmployeesDBinfo.Columns[3] + " = '" + text + "'";
        }

        Console.WriteLine("Введите новую дату рождения в формате DD/MM/YYYY или \"-\" чтобы оставить без изменений:");
        text = Console.ReadLine();
        if (text.ToLower() == "cancel")
            return null;
        DateOnly resDOB = DateOnly.FromDateTime(DateTime.Now);
        while (text != "-" && (!DateOnly.TryParse(text, out resDOB) || !Employee.IsValidDateOfBirth(resDOB)))
        {
            Console.WriteLine("Проверьте корректность введённой даты. Попробуйте еще раз или введите \"Cancel\" для отмены");
            text = Console.ReadLine();
            if (text.ToLower() == "cancel")
                return null;
        }
        if (text != "-")
        {
            if (setfl)
                commandString += ", ";
            setfl = true;
            commandString += $"{EmployeesDBinfo.Columns[4]} = '{resDOB.Year}.{resDOB.Month}.{resDOB.Day}'";

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
            if (setfl)
                commandString += ", ";
            setfl = true;
            commandString += EmployeesDBinfo.Columns[5] + " = " + text;
        }
        if (!setfl)
            return null;
        return commandString;
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
        if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Адрес электроноой почты не может быть пустым");
            return false;
        }
        if (email.Length > 100)
        {
            Console.WriteLine("Слишком длинный адрес электронной почты");
            return false;
        }
        int fl = 1;
        if (!char.IsLetter(email[0]))
        {
            Console.WriteLine("Адрес почты должен начинаться с буквы");
            return false;
        }
        foreach (char let in email)
        {
            switch (fl)
            {
                case 1:
                    if (let == '@')
                        ++fl;
                    else if (!(let == '-' || let == '_' || let == '.' || char.IsLetterOrDigit(let)))
                    {
                        Console.WriteLine("Адрес почты может содержать только буквы, цифры или знаки .-_");

                        return false;
                    }
                    break;
                case 2:
                    if (char.IsLetter(let))
                        ++fl;
                    else
                    {
                        Console.WriteLine("Домен может содержать только буквы");
                        return false;
                    }
                    break;
                case 3:
                    if (let == '.')
                        ++fl;
                    else if (!char.IsLetter(let))
                    {
                        Console.WriteLine("Домен может содержать только буквы");
                        return false;
                    }
                    break;
                case 4:
                    if (char.IsLetter(let))
                        ++fl;
                    else
                    {
                        Console.WriteLine("Домен может содержать только буквы");
                        return false;
                    }
                    break;
                case 5:
                    if (!char.IsLetter(let))
                    {
                        Console.WriteLine("Домен может содержать только буквы");
                        return false;
                    }
                    break;
            }
            
        }
        switch (fl)
        {
            case 1:
                Console.WriteLine("Отсутствует \"@\"");
                return false;
            case 2:
                Console.WriteLine("Отсутствует домен");
                return false;
            case 3:
                Console.WriteLine("Отсутствует \".\" в домене");
                return false;
            case 4:
                Console.WriteLine("Отсутствует код страны в домене");
                return false;
            case 5:
                return true;
        }
        return false;
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


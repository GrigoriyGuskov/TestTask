using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        SqlConnection connection = EmployeesDBinfo.Connect();
        //Console.WriteLine(connection.ClientConnectionId);
        //Console.WriteLine(DBinfo.Connection.ClientConnectionId);

        EmployeesDBinfo.PrintAll();

        if (connection != null)
            connection.Close();
    }
}

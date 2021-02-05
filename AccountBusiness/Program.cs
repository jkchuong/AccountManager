using System;
using System.Linq;
using System.Collections.Generic;
using AccountData;

namespace AccountBusiness
{
    class Program
    {
        static void Main(string[] args)
        {
            Business account = new Business();
            account.Create("Jimmy", "jcsoup", "scrroge");
            Console.WriteLine(account.UserExist("sfdgfd"));
            account.DeleteAllUsers();
        }
    }
}

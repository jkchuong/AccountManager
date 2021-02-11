using System;
using System.Linq;
using System.Collections.Generic;
using AccountData;
using Microsoft.EntityFrameworkCore;

namespace AccountBusiness
{
    class Program
    {
        static void Main(string[] args)
        {
            Business account = new Business();

            account.DeleteAllUsers();

            using var db = new GameContext();

            db.SaveChanges();

        }


    }
}

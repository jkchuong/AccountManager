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
            Random rnd = new Random();

            account.DeleteAllUsers();

            using var db = new GameContext();
            db.Users.Add(new User() { Name = "Jimmy", UserId = "Soup", Password = "Jimmy", Wins = rnd.Next(50, 100), Losses = rnd.Next(30, 100) });
            db.Users.Add(new User() { Name = "Theo", UserId = "Babe", Password = "Theo", Wins = rnd.Next(50, 100), Losses = rnd.Next(30, 100) });
            db.Users.Add(new User() { Name = "Megan", UserId = "Moodle", Password = "Megan", Wins = rnd.Next(50, 100), Losses = rnd.Next(30, 100) });
            db.Users.Add(new User() { Name = "Lorenzo", UserId = "Don", Password = "Lorenzo", Wins = rnd.Next(50, 100), Losses = rnd.Next(30, 100) });
            db.Users.Add(new User() { Name = "Kim", UserId = "Dino", Password = "Kim", Wins = rnd.Next(50, 100), Losses = rnd.Next(30, 100) });
            db.Users.Add(new User() { Name = "Ashib", UserId = "Doc", Password = "Ashib", Wins = rnd.Next(50, 100), Losses = rnd.Next(30, 100) });
            db.Users.Add(new User() { Name = "Adam", UserId = "Scotsman", Password = "Adam", Wins = rnd.Next(50, 100), Losses = rnd.Next(30, 100) });
            db.Users.Add(new User() { Name = "Malik", UserId = "Louis", Password = "Malik", Wins = rnd.Next(50, 100), Losses = rnd.Next(30, 100) });

            db.SaveChanges();

        }


    }
}

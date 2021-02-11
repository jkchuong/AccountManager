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
            Random rnd = new Random();
            db.Add(new User() { Name = "Jimmy", UserId = "Soup", Password = "Jimmy", Wins = rnd.Next(20, 100), Losses = rnd.Next(0, 100) });
            db.Add(new User() { Name = "Theo", UserId = "Babe", Password = "Theo", Wins = rnd.Next(20, 100), Losses = rnd.Next(0, 100) });
            db.Add(new User() { Name = "Megan", UserId = "Moodle", Password = "Megan", Wins = rnd.Next(20, 100), Losses = rnd.Next(0, 100) });
            db.Add(new User() { Name = "Lorenzo", UserId = "Don", Password = "Lorenzo", Wins = rnd.Next(20, 100), Losses = rnd.Next(0, 100) });
            db.Add(new User() { Name = "Adam", UserId = "Scotsman", Password = "Adam", Wins = rnd.Next(20, 100), Losses = rnd.Next(0, 100) });
            db.Add(new User() { Name = "Kim", UserId = "Dinosaur", Password = "Kim", Wins = rnd.Next(20, 100), Losses = rnd.Next(0, 100) });
            db.Add(new User() { Name = "Ashib", UserId = "Doctor", Password = "Ashib", Wins = rnd.Next(20, 100), Losses = rnd.Next(0, 100) });
            db.Add(new User() { Name = "Malik", UserId = "Louis", Password = "Malik", Wins = rnd.Next(20, 100), Losses = rnd.Next(0, 100) });



            db.SaveChanges();

        }


    }
}

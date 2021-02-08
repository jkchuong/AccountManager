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
            //account.DeleteAllUsers();
            //account.CreateUser("Jimmy", "jcsoup", "Password");

            //User user = account.SetSelectedCustomer("jcsoup");
            //Console.WriteLine(user.Name);
            //Console.WriteLine(user.ThemeId);
            //Console.WriteLine(user.Password);

            //account.CreateTheme("White", "Black");
            //account.CreateTheme("Green", "Red");
            //var themes = account.GetThemes();
            //foreach (string theme in themes)
            //{
            //    Console.WriteLine(theme);
            //}

            using (var db = new GameContext())
            {
                var selectedUsers =
                from u in db.Users
                where u.UserId == "jkchuong"
                select u;
                db.Users.RemoveRange(selectedUsers);


                var selectedThemes =
                    from t in db.Themes
                    where t.PrimaryColour == "Purple"
                    select t;
                db.Themes.RemoveRange(selectedThemes);

                db.SaveChanges();
            }

            //using (var db = new GameContext())
            //{
            //    var themes = db.Users.Include(u => u.ThemeId);

            //    foreach (var theme in themes)
            //    {
            //        Console.WriteLine(theme);
            //    }
            //}
        }


    }
}

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
            //    var themes =
            //        from u in db.Users.Include(u => u.Themes)
            //        where u.
            //}
        }


    }
}

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace ManagementCode
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new GameContext())
            {
                db.Users.Add(new User() { Name = "jimmy", UserId = "jcsoup", Password = "scrooge" });
            }
        }
    }
}

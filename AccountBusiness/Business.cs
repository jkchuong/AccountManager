using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AccountData;

namespace AccountBusiness
{
    public class Business
    {
        public User SelectedUser { get; set; }

        public void Create(string name, string userId, string password)
        {
            using (var db = new GameContext())
            {
                db.Add(new User() { Name = name, UserId = userId, Password = password });
                db.SaveChanges();
            }
        }

        public bool UserExist(string username)
        {
            using (var db = new GameContext())
            {
                var entry = db.Users.Find(username);

                if (entry != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool UserAndPasswordExist(string username, string password)
        {
            using (var db = new GameContext())
            {
                var entry =
                    from user in db.Users
                    where user.UserId == username && user.Password == password
                    select user.UserId;

                if (entry.Count() != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void DeleteUser(string userId)
        {
            using (var db = new GameContext())
            {
                var entry = db.Users.Find(userId);
                db.Remove(entry);
                db.SaveChanges();
            }
        }

        public void DeleteAllUsers()
        {
            using (var db = new GameContext())
            {
                db.RemoveRange(db.Users);
                db.SaveChanges();
            }
        }

        public void DeleteAllThemes()
        {
            using (var db = new GameContext())
            {
                db.RemoveRange(db.Themes);
                db.SaveChanges();
            }
        }

        // Can I combine this with UserAndPasswordExist?
        public User SetSelectedCustomer(string username)
        {
            using (var db = new GameContext())
            {
                var entry = db.Users.Find(username);
                return entry;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AccountData;

namespace AccountBusiness
{
    public class Business
    {
        public User selectedUser { get; set; }

        public void CreateUser(string name, string userId, string password)
        {
            using (var db = new GameContext())
            {
                db.Add(new User() { Name = name, UserId = userId, Password = password });
                db.SaveChanges();
            }
        }

        public void CreateTheme(string primary, string secondary)
        {
            using (var db = new GameContext())
            {
                db.Add(new Theme() { PrimaryColour = primary, SecondaryColour = secondary });
                db.SaveChanges();
            }
        }


        public List<string> GetThemePrimary()
        {
            using (var db = new GameContext())
            {
                var primary = db.Themes.Select(t => t.PrimaryColour).ToList();
                return primary;
            }
        }

        public List<string> GetThemeSecondary()
        {
            using (var db = new GameContext())
            {
                var secondary = db.Themes.Select(t => t.SecondaryColour).ToList();
                return secondary;
            }
        }

        public List<string> GetThemes()
        {
            var primary = GetThemePrimary();
            var secondary = GetThemeSecondary();
            List<string> themes = new List<string>();
            for (int i = 0; i < primary.Count; i++)
            {
                themes.Add($"{primary[i]}, {secondary[i]}");
            }

            return themes;
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

        public void UpdateUserNameTheme(string username, string name, int theme)
        {
            using (var db = new GameContext())
            {
                var selectedUser = db.Users.Find(username);
                selectedUser.Name = name;
                selectedUser.ThemeId = theme;
                db.SaveChanges();
            }

        }

        public void UpdatePassword(string username, string password)
        {
            using (var db = new GameContext())
            {
                var selectedUser = db.Users.Find(username);
                selectedUser.Password = password;
                db.SaveChanges();
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

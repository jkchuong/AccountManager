using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AccountData;
using Microsoft.EntityFrameworkCore;

using ChessApp;
using System.IO;
using System.Xml.Linq;

namespace AccountBusiness
{
    public class Business
    {
        public User SelectedUser { get; set; }

        public void CreateUser(string name, string username, string password)
        {
            using var db = new GameContext();
            db.Add(new User() { Name = name, UserId = username, Password = password });
            db.SaveChanges();
        }

        public void CreateTheme(string primary, string secondary)
        {
            using var db = new GameContext();
            db.Add(new Theme() { PrimaryColour = primary, SecondaryColour = secondary });
            db.SaveChanges();
        }


        public List<string> GetThemePrimary()
        {
            using var db = new GameContext();
            var primary = db.Themes.Select(t => t.PrimaryColour).ToList();
            return primary;
        }

        public List<string> GetThemeSecondary()
        {
            using var db = new GameContext();
            var secondary = db.Themes.Select(t => t.SecondaryColour).ToList();
            return secondary;
        }

        public List<string> GetAllThemes()
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

        public List<string> GetUserTheme(string username)
        {
            using var db = new GameContext();
            List<string> userTheme = new List<string>();
            var themes =
                from u in db.Users.Include(u => u.Theme)
                where u.UserId == username
                select new { Primary = u.Theme.PrimaryColour, Secondary = u.Theme.SecondaryColour };

            foreach (var t in themes)
            {
                userTheme.Add(t.Primary);
                userTheme.Add(t.Secondary);
            }
            return userTheme;
        }


        public int AddOneToWins(string username)
        {
            using var db = new GameContext();
            var selectedUser = db.Users.Find(username);
            selectedUser.Wins += 1;
            db.SaveChanges();

            return selectedUser.Wins;
        }

        public int AddOneToLosses(string username)
        {
            using var db = new GameContext();
            var selectedUser = db.Users.Find(username);
            selectedUser.Losses += 1;
            db.SaveChanges();

            return selectedUser.Losses;
        }


        public bool UserExist(string username)
        {
            using var db = new GameContext();
            var selectedUser = db.Users.Find(username);
            if (selectedUser != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UserAndPasswordExist(string username, string password)
        {
            using var db = new GameContext();
            var entry = from user in db.Users where user.UserId == username && user.Password == password select user.UserId;
            if (entry.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateUserNameTheme(string username, string name, int theme)
        {
            using var db = new GameContext();
            var selectedUser = db.Users.Find(username);
            selectedUser.Name = name;
            selectedUser.ThemeId = theme;
            db.SaveChanges();

        }

        public void UpdatePassword(string username, string password)
        {
            using var db = new GameContext();
            var selectedUser = db.Users.Find(username);
            selectedUser.Password = password;
            db.SaveChanges();
        }


        public void DeleteUser(string username)
        {
            using var db = new GameContext();
            var selectedUser = db.Users.Find(username);
            db.Remove(selectedUser);
            db.SaveChanges();
        }

        public void DeleteAllUsers()
        {
            using var db = new GameContext();
            db.RemoveRange(db.Users);
            db.SaveChanges();
        }

        public void DeleteAllThemes()
        {
            using var db = new GameContext();
            db.RemoveRange(db.Themes);
            db.SaveChanges();
        }


        public void SetSaveToExist(string username)
        {
            using var db = new GameContext();
            var selectedUser = db.Users.Find(username);
            selectedUser.SaveExist = true;
            db.SaveChanges();
        }

        public XElement GetUserSave(string userId, XDocument saveFile)
        {
            return 
                saveFile.Descendants("User")
                .Where(s => (string)s.Attribute("UserId") == userId)
                .FirstOrDefault();
        }

        public XDocument LoadSaveFile()
        {
            var filename = "Saves.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var saveFilePath = Path.Combine(currentDirectory, filename);
            XDocument saveFile = XDocument.Load(saveFilePath);
            return saveFile;
        }

        public void DeleteUserSave(string userId, XDocument saveFile)
        {
            saveFile.Descendants().Where(u => (string)u.Attribute("UserId") == userId).FirstOrDefault().Remove();
            saveFile.Save("Saves.xml");
        }

        public void SaveToXML(string userId, Cell[,] board)
        {
            XDocument saveFile = LoadSaveFile();

            var userSave = GetUserSave(userId, saveFile);

            // If user already has save file, delete and create a new one
            if (userSave != null)
            {
                DeleteUserSave(userId, saveFile);
            }

            // Create XElement and populate with pieces
            XElement user = new XElement("User", new XAttribute("UserId", userId));

            foreach (Cell cell in board)
            {
                if (cell.IsOccupied)
                {
                    XElement piece =
                        new XElement("Piece", new XAttribute("Name", cell.Piece.Name),
                            new XElement("Row", cell.Row),
                            new XElement("Column", cell.Column),
                            new XElement("IsWhite", cell.Piece.IsWhite),
                            new XElement("IsFirstMove", cell.Piece.IsFirstMove)
                        );

                    user.Add(piece);
                }
            }

            saveFile.Element("Saves").Add(user);
            saveFile.Save("Saves.xml");
            SetSaveToExist(userId);
        }


        // How to use this when passing user between pages?
        public User SetSelectedCustomer(string username)
        {
            using var db = new GameContext();
            var entry = db.Users.Find(username);
            return entry;
        }

    }
}

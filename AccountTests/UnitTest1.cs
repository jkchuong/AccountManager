using NUnit.Framework;
using AccountBusiness;
using AccountData;
using System.Linq;

namespace AccountTests
{
    public class Tests
    {
        Business _account;
        [SetUp]
        public void Setup()
        {
            _account = new Business();
            using var db = new GameContext();
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

        [TearDown]
        public void TearDown()
        {
            _account = new Business();
            // remove test entry in DB if present
            using var db = new GameContext();
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

        [Test]
        public void WhenAUserIsAdded_NumberOfUsersIncreaseBy1()
        {
            using var db = new GameContext();
            var numberBefore = db.Users.Count();
            _account.CreateUser("Jimmy", "jkchuong", "visual");
            var numberAfter = db.Users.Count();

            Assert.AreEqual(numberBefore + 1, numberAfter);
        }

        [Test]
        public void WhenAThemeIsAdded_NumberOfThemesIncreaseBy1()
        {
            using var db = new GameContext();
            var numberBefore = db.Themes.Count();
            _account.CreateTheme("Purple", "Pink");
            var numberAfter = db.Themes.Count();

            Assert.AreEqual(numberBefore + 1, numberAfter);
        }

        [Test]
        public void WhenUserDetailsAreChanged_DatabaseUpdated()
        {
            using var db = new GameContext();
            _account.CreateUser("Jimmy", "jkchuong", "visual");
            _account.UpdateUserNameTheme("jkchuong", "Brad", 2);

            var updatedUser = db.Users.Find("jkchuong");
            Assert.AreEqual("Brad", updatedUser.Name);
            Assert.AreEqual(2, updatedUser.ThemeId);
        }

        [Test]
        public void WhenUserPasswordIsChanged_DatabaseUpdated()
        {
            using var db = new GameContext();
            _account.CreateUser("Jimmy", "jkchuong", "visual");
            _account.UpdatePassword("jkchuong", "studio");

            var updatedUser = db.Users.Find("jkchuong");
            Assert.AreEqual("studio", updatedUser.Password);
        }
        
        [Test]
        public void IfUserExistReturnTrue()
        {
            using var db = new GameContext();
            _account.CreateUser("Jimmy", "jkchuong", "visual");

            var result = _account.UserExist("jkchuong");
            Assert.AreEqual(true, result);
        }

        [Test]
        public void IfUserDoesntExistReturnFalse()
        {
            using var db = new GameContext();
            _account.CreateUser("Jimmy", "jkchuong", "visual");

            var result = _account.UserExist("akchuong");
            Assert.AreEqual(false, result);
        }

        [Test]
        public void IfUserAndPasswordExistReturnTrue()
        {
            using var db = new GameContext();
            _account.CreateUser("Jimmy", "jkchuong", "visual");

            var result = _account.UserAndPasswordExist("jkchuong", "visual");
            Assert.AreEqual(true, result);
        }

        [Test]
        public void IfUseOrPasswordDoesntExistReturnFalse()
        {
            using var db = new GameContext();
            _account.CreateUser("Jimmy", "jkchuong", "visual");

            var result = _account.UserAndPasswordExist("jkchuong", "studio");
            Assert.AreEqual(false, result);
        }

        [Test]
        public void GetUserThemeReturnsPrimaryAndSecondaryColours()
        {
            using var db = new GameContext();
            _account.CreateUser("Jimmy", "jkchuong", "visual");

            var theme = _account.GetUserTheme("jkchuong");
            Assert.AreEqual("White", theme[0]);
            Assert.AreEqual("Black", theme[1]);
        }

        [Test]
        public void GetUserThemeReturnsPrimaryAndSecondaryColoursAfterUpdatingTheme()
        {
            using var db = new GameContext();
            _account.CreateUser("Jimmy", "jkchuong", "visual");
            _account.UpdateUserNameTheme("jkchuong", "Jimmy", 2);

            var theme = _account.GetUserTheme("jkchuong");
            Assert.AreEqual("Green", theme[0]);
            Assert.AreEqual("Red", theme[1]);
        }
    }
}
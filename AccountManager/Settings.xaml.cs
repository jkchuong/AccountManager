using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Xml.Linq;
using AccountBusiness;
using AccountData;

namespace AccountManager
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        Business _account = new Business();
        User user;

        public Settings(User userPassed)
        {
            user = userPassed;
            var themes = _account.GetAllThemes();

            InitializeComponent();

            // Initialising entries 
            SettingsUsername.Text = user.UserId;
            SettingsName.Text = user.Name;
            AISetting.IsChecked = user.AggressiveOn;
            ThemeBox.SelectedIndex = user.ThemeId - 1;
            ThemeBox.ItemsSource = themes;

        }

        private void Settings_Save_Click(object sender, RoutedEventArgs e)
        {
            string message = "Updated information ";

            // Get info from settings
            string username = user.UserId;
            string newName = SettingsName.Text;
            int newTheme = ThemeBox.SelectedIndex + 1;
            bool aggressiveOn = (bool)AISetting.IsChecked;
            string newPassword = SettingsPassword.Password;
            string newPasswordCheck = SettingsPasswordCheck.Password;

            //ThemeBox.SelectedIndex = user.ThemeId - 1;

            _account.UpdateUserNameTheme(username, newName, aggressiveOn, newTheme);

            if (!String.IsNullOrWhiteSpace(newPassword))
            {
                if (newPassword == newPasswordCheck)
                {
                    _account.UpdatePassword(username, newPassword);
                    message += "and password";
                }
                else
                {
                    message = "Passwords do not match. Nothing updated.";
                }
            }

            Status.Text = message;

            // Reset the current user with updated user
            user = _account.SetSelectedUser(user.UserId);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game(user, false);
            this.NavigationService.Navigate(game);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            XDocument saveFile = _account.LoadSaveFile("Save");
            _account.DeleteUserSave(user.UserId, saveFile, "Save");

            Login login = new Login();
            this.NavigationService.Navigate(login);
            _account.DeleteUser(user.UserId);
        }
    }
}

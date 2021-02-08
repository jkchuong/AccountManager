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
            InitializeComponent();
            user = userPassed;
            SettingsUsername.Text = user.UserId;
            SettingsName.Text = user.Name;

            var themes = _account.GetAllThemes();
            ThemeBox.ItemsSource = themes;

        }

        private void Settings_Save_Click(object sender, RoutedEventArgs e)
        {
            string message = "Updated information ";

            string username = user.UserId;
            string newName = SettingsName.Text;
            int newTheme = ThemeBox.SelectedIndex + 1; // for the PK of themes
            string newPassword = SettingsPassword.Password;
            string newPasswordCheck = SettingsPasswordCheck.Password;

            _account.UpdateUserNameTheme(username, newName, newTheme);

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
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game(user);
            this.NavigationService.Navigate(game);
        }
    }
}

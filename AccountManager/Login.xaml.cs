using System;
using System.Collections.Generic;
using System.Text;
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

namespace AccountManager
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private Business _account = new Business();

        public Login()
        {
            InitializeComponent();
        }

        // Go to registration page
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            this.NavigationService.Navigate(registration);
        }

        // Check if entered information exists, if it does go to Game page, if not show invalid error
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameInput.Text;
            string password = passwordInput.Password;
            bool exists = _account.UserAndPasswordExist(username, password);
            if (exists)
            {
                _account.SetSelectedUser(username);
                Game game = new Game(_account, true);
                this.NavigationService.Navigate(game);
            }
            else
            {
                Message.Text = "Invalid Username or Password";
            }
        }
    }
}

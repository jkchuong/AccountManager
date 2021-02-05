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
using AccountData;

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

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            this.NavigationService.Navigate(registration);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameInput.Text;
            string password = passwordInput.Password;
            bool exists = _account.UserAndPasswordExist(username, password);
            if (exists)
            {
                User user = _account.SetSelectedCustomer(username);
                Game game = new Game(user);
                this.NavigationService.Navigate(game);
            }
            else
            {
                Message.Text = "Invalid Username or Password";
            }
        }
    }
}

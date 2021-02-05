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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Registration : Page
    {
        private Business _account = new Business();

        public Registration()
        {
            InitializeComponent();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            this.NavigationService.Navigate(login);
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string name = ReigstrationName.Text;
            string userId = RegistrationUsername.Text;
            string password = RegistrationPassword.Password;
            string passwordCheck = RegistrationPasswordCheck.Password;

            if (_account.UserExist(userId))
            {
                Test.Text = "Username already exists";
            }
            else 
            {
                if (password == passwordCheck)
                {
                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(password))
                    {
                        Test.Text = "Fields cannot be empty!";
                    }
                    else
                    {
                        _account.Create(name, userId, password);
                        Test.Text = "Registration complete";
                    }
                }
                else
                {
                    Test.Text = "Passwords do not match";
                }
            }


        }
    }
}

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
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        private Business _account = new Business();
        private User user;

        public Game(User userPassed)
        {
            InitializeComponent();
            user = userPassed;
            string data = $"{user.Name}, Wins: {user.Wins}, Losses: {user.Losses}.";
            UserData.Text = data;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            this.NavigationService.Navigate(login);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

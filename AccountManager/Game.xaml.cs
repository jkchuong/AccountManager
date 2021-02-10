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
    /// 

    
    public partial class Game : Page
    {
        private Business _account = new Business();
        private User user;

        public Game(User userPassed)
        {
            user = userPassed;

            InitializeComponent();



            var userThemes = _account.GetUserTheme(user.UserId);
            Brush primaryColour = (SolidColorBrush)new BrushConverter().ConvertFromString(userThemes[0]);
            Brush secondaryColour = (SolidColorBrush)new BrushConverter().ConvertFromString(userThemes[1]);
            CreateButtonGrid(primaryColour, secondaryColour);

            string data = $"{user.Name}, Wins: {user.Wins}, Losses: {user.Losses}.";
            UserData.Text = data;
        }

        private void CreateButtonGrid(Brush primary, Brush secondary)
        {
            for (var col = 0; col < 8; col++)
            {
                var isBlack = col % 2 == 1;
                for (int row = 0; row < 8; row++)
                {
                    var button = new Button() { Background = isBlack ? primary : secondary, Foreground = !isBlack ? primary : secondary };
                    button.Name = "_" + row.ToString() + col.ToString();
                    button.Content = button.Name;
                    button.Click += GridButton_Click;
                    button.FontSize = 30;
                    Chessboard.Children.Add(button);
                    isBlack = !isBlack;
                }
            }
        }

        private void GridButton_Click(object sender, RoutedEventArgs e)
        {
            History.Text = (sender as Button).Name.ToString();
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
            Settings setting = new Settings(user);
            this.NavigationService.Navigate(setting);
        }

        private void Rules_Click(object sender, RoutedEventArgs e)
        {
            if (Rulebook.Visibility == Visibility.Collapsed)
            {
                Rulebook.Visibility = Visibility.Visible;
                Rules.Background = Brushes.Yellow;
            }
            else
            {
                Rulebook.Visibility = Visibility.Collapsed;
                Rules.Background = Brushes.LightGoldenrodYellow;
            }
        }
    }
}

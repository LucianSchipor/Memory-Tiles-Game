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
using System.Windows.Shapes;

namespace MemoryTitlesGame
{
    /// <summary>
    /// Interaction logic for PlayMenu.xaml
    /// </summary>
    public partial class PlayMenu : Window
    {
        public PlayMenu()
        {
            InitializeComponent();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            var gameWindow = new MainWindow();
            gameWindow.DataContext = this.DataContext;
            var userView = gameWindow.DataContext as UserView;
            gameWindow.setSelectedUser((userView.SelectedUser));
            userView.SelectedUser.GamesPlayed++;
            userView.SaveUsersToFile(userView.Users);

           
            gameWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var statsUW = this.DataContext as UserView;

            var statisticsWindow = new Statistics();
            statisticsWindow.DataContext = statsUW;
            statisticsWindow.lblCurrentPlayer.Content = "Salut, " + statsUW.SelectedUser.UserName;
            statisticsWindow.Show();
            this.Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var About = new Window();
            var txt = new TextBox();
            txt.Text = "Schipor Lucian-Alexandru";
            var grupa = new TextBlock();
            grupa.Text = "Grupa 213";

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var gameData = this.DataContext as UserView;
            var signInWindow = new SignIn();
            signInWindow.DataContext = gameData;
            signInWindow.Show();
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var data = this.DataContext as MainWindow;
            var gameData = this.DataContext as UserView;
            var selected = gameData.SelectedUser;
            selected.lastGame = data.buttonContent;
        }
    }
}

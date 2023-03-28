using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MemoryTitlesGame
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        UserView view = new UserView();
        public Statistics()
        {
            InitializeComponent();
        }

        private void YourStatsButton_Click(object sender, RoutedEventArgs e)
        {
            view = (UserView)this.DataContext;
            var selected = view.SelectedUser;
            TextBlock txtGamesWon = new TextBlock();
            txtGamesWon.Text = "You won: " + selected.Wins.ToString() + " games.";
            txtGamesWon.HorizontalAlignment = HorizontalAlignment.Center;
            txtGamesWon.VerticalAlignment = VerticalAlignment.Center;
            txtGamesWon.FontSize = 20;
            txtGamesWon.FontWeight = FontWeights.Bold;

            TextBlock txtGamesPlayed = new TextBlock();
            txtGamesPlayed.Text = "You played: " + selected.GamesPlayed.ToString() + " games.";
            txtGamesPlayed.HorizontalAlignment = HorizontalAlignment.Center;
            txtGamesPlayed.VerticalAlignment = VerticalAlignment.Center;
            txtGamesPlayed.FontSize = 20;
            txtGamesPlayed.FontWeight = FontWeights.Bold;

            GridStats.Children.Add(txtGamesWon);
            GridStats.Children.Add(txtGamesPlayed);
            Grid.SetRow(txtGamesWon, 1);
            Grid.SetRow(txtGamesPlayed, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            view = (UserView)this.DataContext;
            var userslist = view.Users;
            foreach( var user in userslist )
            {

                TextBlock txtGamesWon = new TextBlock();
                txtGamesWon.Text = user.UserName + " played: " + user.GamesPlayed.ToString() + " games and won: " + user.Wins.ToString() + " games.";
                txtGamesWon.FontWeight = FontWeights.Bold;
                txtGamesWon.TextWrapping = TextWrapping.Wrap;
                txtGamesWon.FontSize = 15;
                txtGamesWon.HorizontalAlignment = HorizontalAlignment.Center;

                panelAllStats.Children.Add(txtGamesWon);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var gameData = this.DataContext as UserView;
            var menu = new PlayMenu();
            menu.DataContext = gameData;
            menu.Show();
            this.Close();
        }
    }
}

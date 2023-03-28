using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace MemoryTitlesGame
{
    public partial class MainWindow : Window
    {
        User selectedUser;

        public MainWindow()
        {
            InitializeComponent();
            var userview = (UserView)this.DataContext;
            selectedUser = userview.SelectedUser;
            buttonContent = new List<string>();
            ShuffleButtons();
;

        }


       

        private Button previousButton;
        private String previousContent;

        public List<String> buttonContent;

        private List<string> GetImages()
        {
            string directory = "C:\\Developing\\Repos\\MemoryTitlesGame\\MemoryTitlesGame\\Assets\\";
            //retine TOATE jpg-urile din path
            var images = Directory.GetFiles(directory, "*");

            var models = new List<string>();
            foreach (string index in images)
            {
                models.Add(index);
                models.Add(index);
            }

            return models;
        }
        //Face swap intre doua butoane random
        private void ShuffleButtons()
        {
            Random random = new Random();
            buttonContent = GetImages();
            for (int i = buttonContent.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                string temp = buttonContent[i];
                buttonContent[i] = buttonContent[j];
                buttonContent[j] = temp;
            }
        }

        public void setSelectedUser(User user)
        {
            selectedUser = user;
        }
        public void Button_Click(object sender, RoutedEventArgs e)
        {

            var button = sender as Button;

            //verific daca butonul pe care am apasat este selectat, sau s-a gasit un match pt el
            if (button == previousButton || button.Content.ToString() == "")
            {
                return;
            }

            //daca nu este selectat niciun buton in momentul apasarii,
            //setez butonul apasat ca fiind selectat si ii salvez 
            //content-ul

            if (previousButton == null)
            {
                var bitmapImage = new BitmapImage(new Uri(buttonContent[Grid.GetRow(button) * 4 + Grid.GetColumn(button)]));
                button.Content = new Image() { Source = bitmapImage };

                //afiseaza butonul 
                previousButton = button;
                previousContent = buttonContent[Grid.GetRow(button) * 4 + Grid.GetColumn(button)];
            }
            else
            {
                //daca deja este selectat UN buton
                //verific daca cele doua butoane fac match

                //prima data setez content-ul bun in loc de "?"

                var bitmapImage = new BitmapImage(new Uri(buttonContent[Grid.GetRow(button) * 4 + Grid.GetColumn(button)]));
                button.Content = new Image() { Source = bitmapImage };

                if (CheckMatch(button))
                {
                    previousButton.IsEnabled = false;
                    button.IsEnabled = false;
                }
                else
                {
                    //daca butoanele nu au facut match
                    previousButton.Content = "?";
                    button.Content = "?";
                }

                //am trecut de toate posibilitatile, deci 
                //setez butonul anterior null
                previousButton = null;
                previousContent = "";

                if (CheckLevelEnd())
                {
                    selectedUser.UserCurrentLevel++;
                    if (CheckGameEnd())
                    {
                        MessageBox.Show("Felicitari " + selectedUser.UserName + ", ai castigat inca un joc.");
                        selectedUser.UserCurrentLevel = 1;
                        Game_Won();
                    }
                    else
                    {
                        MessageBox.Show("Felicitari " + selectedUser.UserName + ", ai trecut la nivelul " + selectedUser.UserCurrentLevel);
                        
                        previousButton = null;
                        previousContent = null;
                        var gameWindow = new MainWindow();
                        gameWindow.DataContext = this.DataContext;
                        var userView = gameWindow.DataContext as UserView;
                        gameWindow.setSelectedUser((userView.SelectedUser));
                        userView.UpdateUser(gameWindow.selectedUser);
                        userView.SaveUsersToFile(userView.Users);
                        gameWindow.Show();
                        this.Close();
                    }
                }
            }
        }

        private void Game_Won()
        {
            var gameData = this.DataContext as UserView;

            gameData.UpdateWins(selectedUser);
            foreach (var user in gameData.Users)
            {
                if (user.UserName == selectedUser.UserName)
                {
                    user.Wins = selectedUser.Wins;
                    break;
                }
            }
            gameData.NotifyGameWon();
            if (gameData == null)
            {
                MessageBox.Show("SAL");
                return;
            }
            var signInWindow = new SignIn();
            signInWindow.DataContext = gameData;
            signInWindow.Show();
            this.Close();
        }
        private bool CheckGameEnd()
        {

            if (selectedUser.UserCurrentLevel > 3)
            {
                return true;
            }
            return false;
        }
        private bool CheckLevelEnd()
        {
            //fac verificarea de carti, daca sunt toate afisate cresc level - ul
            foreach (var button in gameGrid.Children)
            {
                if (button is Button && ((Button)button).IsEnabled)
                {
                    return false;
                }
            }
            
            return true;
        }


        private bool CheckMatch(Button button)
        {
            if (previousContent == buttonContent[Grid.GetRow(button) * 4 + Grid.GetColumn(button)])
            {
                return true;
            }
            return false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var content = buttonContent;
            var gameData = this.DataContext as MainWindow;
            var menu = new PlayMenu();
            menu.DataContext = gameData;
            menu.Show();
        }
    }
}

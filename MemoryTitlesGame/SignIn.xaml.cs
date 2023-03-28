using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace MemoryTitlesGame
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        UserView userView = new UserView();
        int profileImageIndex = 0;

        private List<string> signInTitles = new List<string>();
        private List<string> GetImages()
        {
            string directory = "C:\\Developing\\Repos\\MemoryTitlesGame\\MemoryTitlesGame\\ProfileImages\\";
            //retine TOATE jpg-urile din path
            var images = Directory.GetFiles(directory, "*");

            var models = new List<string>();
            int id = 0;
            foreach (string index in images)
            {
                models.Add(index);
            }

            return models;
        }
        public SignIn()
        {
            InitializeComponent();
            signInTitles = GetImages();
        }

        private void GameData_PropertyChanged(object sneder, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Users")
            {
                listUseri.GetBindingExpression(ListBox.ItemsSourceProperty).UpdateTarget();
            }
        }

        public string selectImage(int i)
        {
            if (i > signInTitles.Count)
                i = 0;
            return (signInTitles[i]);
        }

        public static void SaveUsersToFile(List<User> users)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));

            using (FileStream stream = new FileStream("Users.txt", FileMode.Create))
            {
                serializer.Serialize(stream, users);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userView.SelectedUser = (User)listUseri.SelectedItem;
        }
        public void OnChangeImageClick(object sender, RoutedEventArgs e)
        {
            userView.SelectedUser = (User)listUseri.SelectedItem;
            // Deschide o casetă de dialog pentru selectarea unei imagini
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fișiere imagine (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                // Actualizează sursa imaginii pentru utilizatorul selectat
                var selectedUser = (User)userView.SelectedUser;
                selectedUser.setProfileImage(openFileDialog.FileName);
                userView.UpdateUser(selectedUser);
                SaveUsersToFile(userView.Users);
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            userView.SelectedUser = (User)listUseri.SelectedItem;
            if (userView.verifyPassword(userView.SelectedUser))
            {


                if (userView.SelectedUser == null)
                {
                    MessageBox.Show("Nu ai selectat niciun utilizator.");
                    return;
                }
                var PlayMenu = new PlayMenu();
                PlayMenu.DataContext = this.DataContext;
                PlayMenu.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Parola incorecta");
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            userView.SelectedUser = (User)listUseri.SelectedItem;
            if (userView.SelectedUser == null)
            {
                MessageBox.Show("Nu ai selectat niciun cont.");
                return;
            }

            MainWindow gameWindow = new MainWindow();
            gameWindow.setSelectedUser((User)(listUseri).SelectedItem);
            var gameData = new UserView();
            gameWindow.DataContext = this.DataContext;
            gameWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            // Crearea unei ferestre de dialog pentru a solicita numele utilizatorului
            var dialog = new InputDialog("Enter username:");
            var username = " "; 
            if (dialog.ShowDialog() == true)
            {
                // Crearea unui nou utilizator și adăugarea sa la lista de utilizatori a jocului
                username = dialog.Answer;

                
            }

            var dialogPass = new InputDialog("Enter password");
            var pass = " ";
            if(dialogPass.ShowDialog() == true)
            {
                pass = dialogPass.Answer;
            }

            //aici imi da lista proasta
            var gameData = (UserView)this.DataContext;
            gameData.addNewUser(username, pass);
            SaveUsersToFile(gameData.Users);
            listUseri.ItemsSource = null;
            listUseri.ItemsSource = gameData.Users;
        }

        public static List<User> LoadUsersFromFile()
        {
            List<User> users = new List<User>();

            if (File.Exists("Users.txt"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                using (FileStream stream = new FileStream("Users.txt", FileMode.Open))
                {
                    users = (List<User>)serializer.Deserialize(stream);
                }
            }

            return users;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var view = this.DataContext as UserView;
            if (view.SelectedUser == null)
            {
                MessageBox.Show("Nu ai selectat niciun user");
                return;
            }
            else
            {
                if (view.verifyPassword(view.SelectedUser) == false)
                    return;
            }
            string filePath = "Users.txt";

            // Deserializam lista cu toti userii din fisierul XML
            List<User> users = new List<User>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
            using (StreamReader reader = new StreamReader(filePath))
            {
                users = (List<User>)serializer.Deserialize(reader);
            }
            // Cautam utilizatorul pe care dorim sa il stergem si il eliminam din lista
            User userToDelete = users.FirstOrDefault(u => u.Id == view.SelectedUser.Id);
            int pos = userToDelete.Id;
            if (userToDelete != null)
            {
                users.Remove(userToDelete);
            }

            // Serializam lista actualizata si o scriem in fisierul XML
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, users);
            }

                for (int i = pos; i < users.Count; i++)
                {
                    users[i].Id--;
                }
            var gamedata = this.DataContext as UserView;
            gamedata.Users = users;
            SaveUsersToFile(users);
            listUseri.ItemsSource = users;
            MessageBox.Show("User-ul a fost sters cu succes!");
        }
    }
}

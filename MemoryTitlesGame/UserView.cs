using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace MemoryTitlesGame
{
    public class UserView : INotifyPropertyChanged
    {

        public event EventHandler GameWon;

        public void NotifyGameWon()
        {
            GameWon?.Invoke(this, EventArgs.Empty);
        }

        private User _selectedUser;
        public User SelectedUser {
            get { 
                return _selectedUser; 
            }
            set { 
                _selectedUser = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedUser)));
            }
        }


        private List<string> profileImages = new List<string>();
        public UserView()
        {
            Users = LoadUsersFromFile();
            profileImages = GetImages();
        }


        private List<User> _users;
        public List<User> Users
        {
            get { 
                return _users; 
            }
            set { _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private List<string> GetImages()
        {
            string directory = "C:\\Developing\\Repos\\MemoryTitlesGame\\MemoryTitlesGame\\ProfileImages\\";
            //retine TOATE jpg-urile din path
            var images = Directory.GetFiles(directory, "*");

            var models = new List<string>();
            foreach (string index in images)
            {
                models.Add(index);
            }
            return models;
        }

        public void UpdateWins(User user)
        {
            user.Wins++;
            UpdateUser(user);
            SaveUsersToFile(Users);
        }


        public bool verifyPassword(User user)
        {
            string pass;
            var dialog = new InputDialog("Enter Password: ");
            
            if (dialog.ShowDialog() == true)
            {
                // Crearea unui nou utilizator și adăugarea sa la lista de utilizatori a jocului
                pass = dialog.Answer;
                if(pass != user.Password)
                {
                    return false;
                }
            }
            return true;
        }
        public void addNewUser(string name, string password)
        {
            User newUser = new User(profileImages);
            newUser.UserName = name;
            newUser.Password = password;
            if(Users.Count() != 0)
                newUser.Id = Users[Users.Count()-1].Id + 1;
            else
                newUser.Id = 0;
            Users.Add(newUser);
            OnPropertyChanged("Users");
            MessageBox.Show("User-ul " + newUser.UserName + " a fost creat.");

            LoadUsersFromFile();
            SaveUsersToFile(Users);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public void modifyWrongIDs(int startPos)
        {
            for (int i = startPos; i < Users.Count; i++)
            {
                Users[i].Id--;
                OnPropertyChanged("Id");
            }
        }
        public void SaveUsersToFile(List<User> users)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));

            using (FileStream stream = new FileStream("Users.txt", FileMode.Create))
            {
                serializer.Serialize(stream, users);
            }
        }

        public void UpdateUser(User userToUpdate)
        {
            User userInList = Users.FirstOrDefault(u => u.Id == userToUpdate.Id);
            if (userInList != null)
            {
                userInList.UserName = userToUpdate.UserName;
                userInList.UserCurrentLevel = userToUpdate.UserCurrentLevel;
                userInList.ProfileImage = userToUpdate.ProfileImage;
            }
        }
    }
}

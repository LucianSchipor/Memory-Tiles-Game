using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MemoryTitlesGame
{
    [Serializable]
    public class User : INotifyPropertyChanged
    {
        List<string> signInTitles;
        private string username;
        private string profileImage;
        protected int _wins;
        private int _userCurrentLevel;
        private int _id;
        private int _gamesPlayed;
        public List<string> lastGame;
        protected string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public int GamesPlayed
        {
            get { return _gamesPlayed; }
            set
            {
                _gamesPlayed = value;                
            }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public int UserCurrentLevel
        {
            get
            {
                return _userCurrentLevel;
            }
            set
            {
                _userCurrentLevel = value;
                NotifyPropertyChanged(nameof(UserCurrentLevel));
            }
        }

        public int Wins
        {
            get { return _wins; }
            set {
                _wins = value;
                NotifyPropertyChanged("Wins");
            }
        }

        public string UserName
        {
            get
            { return username; }
            set { username = value; }
        }

        public string ProfileImage
        {
            get { return profileImage; }
            set { profileImage = value; }
        }
        public User(List<string> signInTitles)
        {
            this.signInTitles = signInTitles;
            this.UserCurrentLevel = 1;
            setProfileImage();
        }

        public User(string Username, List<string> signInTitles)
        {
            this.UserName = Username;
            this.UserCurrentLevel = 1;
            this.Id = 1;
            setProfileImage();
        }

        public User()
        {
            //pentru functia de serializare am nevoie de un constructor gol
        }


        private void setProfileImage()
        {
            if (profileImage == null)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, signInTitles.Count() - 1);
                profileImage = signInTitles[randomNumber];
            }
        }

        public void setProfileImage(string path)
        {
            this.profileImage = path;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ProfileImage)));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

       
    }
}

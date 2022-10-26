using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace Walkydoggy.Models
{
    public class User : Notifier
    {
        private string _RuDog;

        public String RuDog
        {
            get
            {
                return _RuDog;
            }
            set
            {
                _RuDog = value;
                OnPropertyChanged("RuDog");
            }
        }


        //blob 타입 이미지 
        private byte[] _Image;

        public byte[] Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
                OnPropertyChanged("Image");
            }
        }




        private string _Bio;

        public String Bio
        {
            get
            {
                return _Bio;
            }
            set
            {
                _Bio = value;
                OnPropertyChanged("Bio");
            }
        }


        private string _Pw;

        public String Pw
        {
            get
            {
                return _Pw;
            }
            set
            {
                _Pw = value;
                OnPropertyChanged("Pw");
            }
        }



        private string _Id;

        public String Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _type;

        public string Type
        {
            get { return this._type; }
            set
            {
                this._type = value;
                OnPropertyChanged("Type");
            }
        }



        private string _Name;

        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        //로그인 
        public User(string id, string pw)
        {
            Id = id;
            Pw = pw;
        }



        // 회원가입 ( 사진 없이 )  
        public User(string rudog, string id, string pw, string username, string bio)
        {
            RuDog = rudog;
            Id = id;
            Pw = pw;
            Name = username;
            Bio = bio;
        }
        //회원가입 (사진이랑)
        public User(string rudog, string id, string pw, string username, byte[] img, string bio)
        {
            RuDog = rudog;
            Id = id;
            Pw = pw;
            Name = username;
            Image = img;
            Bio = bio;
        }

        //강아지인지 사람인지 구분 
        public User(string v)
        {
            RuDog = v;
        }



        public User(User _User)
        {

        }

        public User()
        {
        }
    }
}

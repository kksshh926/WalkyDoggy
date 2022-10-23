using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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
using Walkydoggy.Commands;
using System.Linq;
using System.Threading.Tasks;
using Walkydoggy.Models;


namespace Walkydoggy.ViewModels
{

    internal class UserViewModel : ViewModelBase
    {

        private string _RuDog;
        private byte[] _Image;
        private string _Bio;
        private string _Pw;
        private string _Id;
        private string _Name;
        private string _Email;
        private string _KakaoId;


        public String Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                SetProperty<string>(ref _Email, value);
            }
        }
        public String KakaoId
        {
            get
            {
                return _KakaoId;
            }
            set
            {
                _KakaoId = value;
                SetProperty<string>(ref _KakaoId, value);
            }
        }
        public String RuDog
        {
            get
            {
                return _RuDog;
            }
            set
            {
                _RuDog = value;
                SetProperty<string>(ref _RuDog, value);
            }
        }

        public String Bio
        {
            get
            {
                return _Bio;
            }
            set
            {
                SetProperty<string>(ref _Bio, value);
            }
        }



        public String Pw
        {
            get
            {
                return _Pw;
            }
            set
            {
                SetProperty<string>(ref _Pw, value);
            }
        }




        public String Id
        {
            get
            {
                return _Id;
            }
            set
            {
                SetProperty<string>(ref _Id, value);
            }
        }




        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetProperty<string>(ref _Name, value);
            }
        }
        string Conn = "SERVER=kksshh926.cafe24.com;DATABASE=kksshh926;UID=kksshh926;PASSWORD=sojin0713!";
        //새 인스턴스 초기화 
        public UserViewModel()
        {
            //기본생성자
            _User = new User();
            //회원가입문
            UpdateCommand = new UserUpdateCommand(this);
            //회원가입중 어떤 회원으로 등록할것인지
            Whoiam = new User[]
            {
                new User("반려견"),
                new User("산책도우미")
            };


        }
        public INavigation Navigation { get; set; }

        public UserViewModel(INavigation navigation)
        {
            /*    Title = "Matching ";
                Navigation = navigation;
                LoginClickCommand = new UserLoginCommand(() => ExecuteLoginClickCommand());*/

        }

        // 업뎃 가능한 상태인지 나타내는거 
        public bool CanUpdate
        {
            get
            {
                if (User == null) //아무것도 안적으면 업뎃 ㄴㄴ 못함
                {
                    return false;
                }
                return true;
            }

        }


        private User _User;


        public User User
        {
            get
            {
                return _User;
            }
        }

        public ICommand UpdateCommand
        {
            get;
            private set;
        }





        //회원가입
        public void SaveChanges()
        {
            // Debug.Assert(false, String.Format("{0} was a updated.", User.Name));

            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                try
                {
                    conn.Open();
                    MySqlCommand msc = new MySqlCommand("INSERT INTO USERS(id,pw,name,image,bio,type) values('" + User.Id + "','" + User.Pw + "','" + User.Name + "','" + User.Image + "','" + User.Bio + "','" + User.RuDog + "');", conn);
                    msc.ExecuteNonQuery();
                    MessageBox.Show("회원가입 완료");
                }
                catch(Exception)
                {
                    MessageBox.Show("다시 시도해 주세요.");
                }
            }
        }



        // 시작: 회원가입할떄 당신 강아지인지 사람인지 설정 
        private IEnumerable<User> _whoiam;
        public IEnumerable<User> Whoiam
        {
            get { return _whoiam; }
            set
            {
                // Some logic here
                _whoiam = value;
                SetProperty<IEnumerable<User>>(ref _whoiam, value);

                OnPropertyChanged("Whoiam");
            }
        }




        private User _selectwho;
        public User Selectedwho
        {
            get
            {
                return _selectwho;
            }
            set
            {
                _selectwho = value;
                OnPropertyChanged("Selectedwho");
                OnselectedChanged();
            }
        }

        public bool CanLogin { get; internal set; }

        private void OnselectedChanged()
        {
            decidedwho();
        }

        private void decidedwho()
        {
            if (Selectedwho == null)
                return;
            User.RuDog = Selectedwho.RuDog;
        }
        //끝: 강아지 or 사람 설정 

    }
}
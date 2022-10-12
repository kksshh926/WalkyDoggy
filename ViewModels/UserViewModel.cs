using Microsoft.EntityFrameworkCore.Metadata;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Input;
using Walkydoggy.Commands;
using Walkydoggy.Models;

namespace Walkydoggy.ViewModels
{

    internal class UserViewModel : Notifier
    {
        string Conn = "SERVER=localhost;DATABASE=walkydog;UID=root;PASSWORD=201933043";
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
                conn.Open();
                MySqlCommand msc = new MySqlCommand("INSERT INTO user(rudog,id,pw,name,bio) values('" + User.RuDog + "','" + User.Id + "','" + User.Pw + "','" + User.Name + "','" + User.Bio + "');", conn);
                // MySqlCommand msc = new MySqlCommand("INSERT INTO user(rudog,id,pw,name,bio) values('a','b','c','f','f');", conn); 잘들어가는것 확인
                msc.ExecuteNonQuery();
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

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Walkydoggy.Commands;
using Walkydoggy.Models;
using Walkydoggy.ViewModels;
using Walkydoggy.Views;


namespace Walkydoggy.ViewModels
{

    internal class LoginViewModel : Notifier
    {
        string Conn = "SERVER=kksshh926.cafe24.com;DATABASE=kksshh926;UID=kksshh926;PASSWORD=sojin0713!";
        public LoginViewModel(Page page)
        {
            _User = new User();

            page1 = page;
            // page.NavigationService.Navigate(new Uri("Views/Matching.xaml", UriKind.Relative));
            //로긴
            //LoginCommand = new UserLoginCommand(this);
            /*LoginCommand = new RelayCommand(ShowMessage, param => canExecute); 버튼이벤트 함수  */

            LoginCommand = new Command(_execute, _canexecute);
            // RegisterClickCommand = new Command(async () => await ExecuteRegisterClickCommand());


        }

        public bool _canexecute(object parameter)
        {
            return true;
        }

        Page page1;
        private void _execute(object arg)
        {
            bool b = FindUser();
            if (b)
            {
                page1.NavigationService.Navigate(new Uri("Views/Paging.xaml", UriKind.Relative));

            }

        }

        public Command RegisterClickCommand { get; set; }

        public Command LoginCommand { get; set; }


        public bool FindUser()
        {
            string userid = User.Id;
            string userpw = User.Pw;
            if (userid == "" || userpw == "")
                return false;
            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                DataSet ds = new DataSet();
                string sql = "SELECT id,pw,rudog,name,bio,image FROM USER WHERE id = '" + User.Id + "' and pw = '" + User.Pw + "';";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "user");
                try
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        User.RuDog = ds.Tables[0].Rows[0]["RuDog"].ToString();
                        User.Id = ds.Tables[0].Rows[0]["Id"].ToString();
                        User.Pw = ds.Tables[0].Rows[0]["Pw"].ToString();
                        string v = ds.Tables[0].Rows[0]["Name"].ToString();
                        User.Name = v;
                        User.Bio = ds.Tables[0].Rows[0]["Bio"].ToString();
                        if (userid == User.Id && userpw == User.Pw)
                            //값 잘 받는것 확인 

                            return true;
                    }

                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
                return false;
            }

        }

        public bool CanLogin
        {
            get
            {


                if (User == null)
                {
                    return false;
                }
                return true;
            }
        }


        /*
                private void ShowMessage(object obj)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show(obj.ToString());
                }

                private void ChangeCanExecute(object obj)
                {
                    canExecute = !canExecute;
                }

           */






        private User _User;


        public User User
        {
            get
            {
                return _User;
            }
        }




        // ViewModel 에서 버튼 이벤트 만들기 함수들 ㅠㅠ  버튼이벤트되면 메소드가 안되고 메소드되면 버튼이벤트가 안되고 
        // //Q1.  버튼 하나로 메소드랑 ㅇ벤트 같이 못하는지 여쭤보기
        // private ICommand loginCommand;

        /*
                private bool canExecute = true;
                public string LoginButtonContent
                {
                    get
                    {
                        return "Login !";
                    }
                }

                public bool CanExecute
                {
                    get
                    {
                        return this.canExecute;
                    }

                    set
                    {
                        if (this.canExecute == value)
                        {
                            return;
                        }

                        this.canExecute = value;
                    }
                }*/










    }
}

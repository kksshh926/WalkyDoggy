using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using Walkydoggy.Models;
using Walkydoggy.ViewModels;

namespace Walkydoggy.View
{

    /// <summary>
    /// Login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Page
    {
        string Conn = "SERVER=kksshh926.cafe24.com;DATABASE=kksshh926;UID=kksshh926;PASSWORD=sojin0713!";
        private UserViewModel userViewModel = new UserViewModel();

        public Login()
        {
            InitializeComponent();
            this.DataContext = this.userViewModel;
        }
        public bool FindUser()
        {
            string userid = this.userViewModel.Id;
            string userpw = this.userViewModel.Pw;
            if (userid == "" || userpw == "")
                return false;
            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                DataSet ds = new DataSet();
                string sql = "SELECT id,pw,type,name,bio,image FROM USERS WHERE id = '" + userViewModel.Id + "' and pw = '" + userViewModel.Pw + "';";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "users");
                try
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        this.userViewModel.RuDog = ds.Tables[0].Rows[0]["Type"].ToString();
                        this.userViewModel.Id = ds.Tables[0].Rows[0]["Id"].ToString();
                        this.userViewModel.Pw = ds.Tables[0].Rows[0]["Pw"].ToString();
                        string v = ds.Tables[0].Rows[0]["Name"].ToString();
                        this.userViewModel.Name = v;
                        this.userViewModel.Bio = ds.Tables[0].Rows[0]["Bio"].ToString();
                        if (userid == this.userViewModel.Id && userpw == this.userViewModel.Pw)
                        {
                            Uri uri = new Uri("/Views/Paging.xaml", UriKind.Relative);
                            NavigationService.Navigate(uri);
                            MessageBox.Show("로그인 성공.");
                            return true;

                        }//값 잘 받는것 확인 
                           // return false;
                    }
                    MessageBox.Show("없는 정보입니다. 다시 확인해주세요.");
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            FindUser();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Views/Register.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }
        private void btn_Reset_ID_Click(object sender, RoutedEventArgs e)
        {
            Walkydoggy.Views.Find_ID find_ID = new Walkydoggy.Views.Find_ID();
            find_ID.Show();
        }
        private void btn_Reset_PSSWD_Click(object sender, RoutedEventArgs e)
        {
            Walkydoggy.View.Find_PSSWD find_PSSWD = new Walkydoggy.View.Find_PSSWD();
            find_PSSWD.Show();
        }

    }
}

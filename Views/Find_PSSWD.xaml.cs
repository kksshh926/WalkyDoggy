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
    /// Find_PSSWD.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Find_PSSWD : Window
    {
        string Conn = "SERVER=kksshh926.cafe24.com;DATABASE=kksshh926;UID=kksshh926;PASSWORD=sojin0713!";
        private UserViewModel userViewModel = new UserViewModel();

        public Find_PSSWD()
        {
            InitializeComponent();
            this.DataContext = this.userViewModel;

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
        private void btn_Find_PSSWD_Click(object sender, RoutedEventArgs e)
        {
            FindPw();
        }

        public bool FindPw()
        {
            string useremail = this.userViewModel.Email;
            string userKakaoId = this.userViewModel.KakaoId;
            if (useremail == "" || userKakaoId == "")
                return false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Conn))
                {
                    conn.Open();

                    DataSet ds = new DataSet();
                    string sql = "SELECT pw FROM USERS WHERE email = '" + userViewModel.Email + "' and kakaoid = '" + userViewModel.KakaoId + "';";
                    MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                    adpt.Fill(ds, "users");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("비밀번호는: " + ds.Tables[0].Rows[0]["pw"] + " 입니다.");
                    }
                    else
                        throw new Exception("없는 정보입니다. 다시 확인해주세요.");

                    conn.Close();
                    this.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }
    }
}

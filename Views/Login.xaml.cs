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
                string sql = "SELECT id,pw,type,name,bio,image FROM USERS WHERE id = '" + userid + "' and pw = '" + userpw + "';";
                MySqlDataAdapter adpt = new MySqlDataAdapter(sql, conn);
                adpt.Fill(ds, "users");

                try
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {


                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //{

                        var row = ds.Tables[0].Rows[0];
                        //의미없는 코드 주석처리
                        //this.userViewModel.RuDog = row["Type"].ToString();
                        //this.userViewModel.Id = row["Id"].ToString();
                        //this.userViewModel.Pw = row["Pw"].ToString();
                        //string v = ds.Tables[0].Rows[0]["Name"].ToString();
                        //this.userViewModel.Name = v;
                        //this.userViewModel.Bio = ds.Tables[0].Rows[0]["Bio"].ToString();
                        //if (userid == this.userViewModel.Id && userpw == this.userViewModel.Pw)
                        //{

                        //웹상 세션과 동일한 역활을 하기위한 정보 저장
                        //실무에서는 로그인 API 에서 Access Token, Refresh Token 등 발급받아 클라이언트에서 발급받은 Token 정보로 Http 프로토콜에 Authorization 헤더 추가 후 API 통신하는것을 배워보세요
                        //Mqtt Topic 에 사용될 유저 정보니까 Mqtt에 Topic 을 어떻게 생성하는지 이 프로젝트에서 Subscript 쪽을 분석해보세요
                        Common.UserInfo = new User
                        {
                            //Type = (string)row["type"],
                            Id = row["id"].ToString(),
                            Image = row["image"] == DBNull.Value ? null : (byte[])row["image"],
                            Name = row["name"].ToString(),
                            Bio = row["bio"].ToString()
                        };

                        //로그인 성공시 브로커 서버 초기화
                        MyMqtt.M2MqttInitializeClient();

                        if (userid == "Admin" && userpw == "Admin")
                        {
                            Uri uri = new Uri("/Views/Admin.xaml", UriKind.Relative);
                            NavigationService.Navigate(uri);
                        }
                        else
                        {
                            Uri uri = new Uri("/Views/Paging.xaml", UriKind.Relative);
                            NavigationService.Navigate(uri);
                        }
                        //MessageBox.Show("로그인 성공.");
                        return true;

                        //}
                    }
                    else
                        throw new Exception("없는 정보입니다. 다시 확인해주세요.");
                    
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
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

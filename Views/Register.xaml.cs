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
using Walkydoggy.ViewModels;


namespace Walkydoggy.View
{
    /// <summary>
    /// Register.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Register : Page
    {
        string Conn = "SERVER=kksshh926.cafe24.com;DATABASE=kksshh926;UID=kksshh926;PASSWORD=sojin0713!";
        private UserViewModel userViewModel = new UserViewModel();
        public Register()
        {
            InitializeComponent();
            this.DataContext = this.userViewModel;
        }

        public object Id { get; private set; }

        //사진 업로드
        private void PhotoUpload_btn(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                if (openFileDialog.ShowDialog() == true)
                {
                    this.txt_PhotoPath.Text = openFileDialog.FileName;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("사진 등록 오류");
            }
        }
        bool CheckData(object sender)
        {
            if (!userViewModel.Whoiam.Any())
            {
                MessageBox.Show("반려견 혹은 산책도우미로 유저 타입을 입력해주세요");
                return false;
            }
            if (this.userViewModel.Id == null)
            {
                MessageBox.Show("아이디를 입력해주세요");
                return false;
            }
            else if (this.userViewModel.Pw == null)
            {
                MessageBox.Show("비밀번호를 입력해주세요");
                return false;
            }
            else if (this.userViewModel.Name == null)
            {
                MessageBox.Show("이름을 입력해주세요");
                return false;
            }
            else if (this.userViewModel.Email == null)
            {
                MessageBox.Show("이메일을 입력해주세요");
                return false;
            }
            else if (this.userViewModel.KakaoId == null)
            {
                MessageBox.Show("카카오 아이디를 입력해주세요");
                return false;
            }
            return true;
        }
        private void SignUp_btn(object sender, RoutedEventArgs e)
        {
            bool result = CheckData(sender);

            if (result)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(Conn))
                    {
                        conn.Open();
                        using (var msc = conn.CreateCommand())
                        {

                            msc.CommandText = $@"INSERT INTO USERS(id,pw,name,image,bio,type,email,kakaoid) 
                                            values(@id, @pw, @name, @image, @bio, @type, @email, @kakaoid)";
                            if (File.Exists(this.txt_PhotoPath.Text))
                            {
                                byte[] binary = File.ReadAllBytes(this.txt_PhotoPath.Text);
                                msc.Parameters.Add(new MySqlParameter("image", binary));
                            }
                            else
                            {
                                msc.Parameters.Add(new MySqlParameter("image", null));
                            }
                            msc.Parameters.Add(new MySqlParameter("id", this.userViewModel.Id));
                            msc.Parameters.Add(new MySqlParameter("pw", this.userViewModel.Pw));
                            msc.Parameters.Add(new MySqlParameter("name", this.userViewModel.Name));
                            msc.Parameters.Add(new MySqlParameter("bio", this.userViewModel.Bio));
                            msc.Parameters.Add(new MySqlParameter("type", this.userViewModel.RuDog));
                            msc.Parameters.Add(new MySqlParameter("email", this.userViewModel.Email));
                            msc.Parameters.Add(new MySqlParameter("kakaoid", this.userViewModel.KakaoId));


                            if (msc.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("회원가입 완료");
                            }
                            else
                            {
                                throw new Exception("등록오류");
                            }

                            NavigationService.GoBack();
                        }
                        conn.Close();
                    }

                }
                catch (Exception)
                { }
            }

        }
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Walkydoggy.Views;
using static Walkydoggy.ViewModels.UserViewModel;

namespace Walkydoggy.Views
{
    /// <summary>
    /// Admin.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Admin : Page
    {

        private List<UserItem> userList;
        private UserViewModel viewModel = new UserViewModel();

        public Admin()
        {
            InitializeComponent();
            this.DataContext = this.viewModel;
            this.LoadUserList();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //셀 선택시 각 정보 텍스트 박스에 넘어가도록
            try
            {

                UserItem myrow = (UserItem)membersDataGrid.CurrentCell.Item;

                Id.Text = myrow.Id.ToString();
                Name.Text = myrow.Name.ToString();
                Email.Text = myrow.Email.ToString();
                KakaoId.Text = myrow.KakaoId.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("예외처리오류");
            }

        }

        private void LoadUserList()
        {
            try
            {

                this.userList = new List<UserItem>();

                using (var conn = new MySqlConnection(Common.ConnectionString))
                {
                    conn.Open();

                    using (var cmb = conn.CreateCommand())
                    {
                        //게시판 조회 쿼리
                        cmb.CommandText = $@"select id, NAME, email, kakaoId from USERS";

                        using (var read = cmb.ExecuteReader())
                        {
                            while (read.Read())
                            {

                                //전역변수에 해당날짜 게시물 수집
                                this.userList.Add(new UserItem
                                {

                                    Id = read["id"].ToString(),
                                    Name = read["name"].ToString(),
                                    Email = read["email"].ToString(),
                                    KakaoId = read["kakaoid"].ToString()

                                });
                            }

                            read.Close();
                        }
                    }

                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.SetBoardList();
            }
        }

        /// <summary>
        /// 검색어에 따라 Binding된 리스트를 셋팅하는 메소드
        /// </summary>
        private void SetBoardList()
        {
            try
            {
                //string conString = "SERVER=kksshh926.cafe24.com;DATABASE=kksshh926;UID=kksshh926;PASSWORD=sojin0713!";
                //MySqlConnection con = new MySqlConnection(conString);
                //MySqlCommand command = new MySqlCommand("select id, NAME, email, kakaoId from USERS Text;", con);
                //con.Open();
                //DataTable dtG = new DataTable();
                //dtG.Load(command.ExecuteReader());
                //List<UserItem> userList = dtG.DataTableToList<UserItem>();
                //MySqlDataAdapter sdr = new MySqlDataAdapter(command);
                //sdr.Fill(dtG);
                //membersDataGrid.ItemsSource = dtG.DefaultView;
                //membersDataGrid.DataContext = userList;
                //con.Close();
                //this.LoadUserList();
                this.viewModel.UserList = userList;


            }
            catch (Exception e)
            {
                MessageBox.Show("false");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = @"SERVER=kksshh926.cafe24.com;DATABASE=kksshh926;UID=kksshh926;PASSWORD=sojin0713!";
            try
            {


                using (var conn = new MySqlConnection(connectionString))
                {
                    using (var msc = conn.CreateCommand())
                    {
                        conn.Open();
                        msc.CommandText = $@"DELETE b,u from USERS as u inner join BOARD as b on b.id = u.id where b.id='" + this.Id.Text + "';";
                        msc.Parameters.AddWithValue("@id", Id);
                        msc.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL error" + ex.Message);
            }
        }
    }
}

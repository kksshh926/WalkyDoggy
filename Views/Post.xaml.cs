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
using Walkydoggy.Models;
using Walkydoggy.ViewModels;

namespace Walkydoggy.View
{
    /// <summary>
    /// Post.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Post : Page
    {
        private PostViewModel viewModel = new PostViewModel();

        public Post()
        {
            InitializeComponent();

            this.DataContext = this.viewModel;

            this.InitTime();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void InitTime()
        {
            List<int> years = new List<int>();
            List<int> months = new List<int>();
            List<int> days = new List<int>();
            var now = DateTime.Now;

            try
            {
                for (int i = DateTime.Now.Year - 5; i < DateTime.Now.Year + 5; i++)
                    years.Add(i);
                for (int i = 1; i <= 12; i++)
                    months.Add(i);

                var total = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

                for (int i = 1; i <= total; i++)
                    days.Add(i);

                this.viewModel.Whatyear = years;
                this.viewModel.Whatmonth = months;
                this.viewModel.Whatday = days;

                this.viewModel.Selectyear = now.Year;
                this.viewModel.Selectmonth = now.Month;
                this.viewModel.Selectday = now.Day;
            }
            catch (Exception)
            { 
            
            }
        }

        private void RefreshDay()
        {
            List<int> days = new List<int>();
            var now = DateTime.Now;

            try
            {
                if (this.viewModel.Selectyear != null && this.viewModel.Selectday != null)
                {
                    var total = DateTime.DaysInMonth(this.viewModel.Selectyear.Value, this.viewModel.Selectmonth.Value);

                    for (int i = 1; i <= total; i++)
                        days.Add(i);
                }
            }
            catch (Exception)
            { 
            
            }
            finally
            {
                this.viewModel.Whatday = days;
            }
        }

        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                
                if (openFileDialog.ShowDialog() == true)
                {
                    this.txt_PhotoPath.Text = openFileDialog.FileName;
                }
            }
            catch(Exception)
            {

            }
        }

        private void cmb_Year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void cmb_Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
                this.RefreshDay();
        }

        private void btnRegister_Complete_Click(object sender, RoutedEventArgs e)
        {
            bool? plasticBag = null;

            try
            {
                if (this.toilet_bag_yes.IsChecked.Value)
                    plasticBag = true;
                else if (this.toilet_bag_no.IsChecked.Value)
                    plasticBag = false;


                //notnull col check
                if (plasticBag == null)
                    MessageBox.Show("배변봉투 여부를 선택하세요.");
                else if (!this.viewModel.Selectyear.HasValue ||
                    !this.viewModel.Selectmonth.HasValue ||
                    !this.viewModel.Selectday.HasValue)
                    MessageBox.Show("날짜를 선택해주세요.");
                else if (string.IsNullOrEmpty(this.viewModel.DogName))
                    MessageBox.Show("이름을 입력해주세요.");

                //유효성 체크
                using (var conn = new MySqlConnection(Common.ConnectionString))
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $@"INSERT INTO BOARD (post, imagefile, id, date_published, date, age, breed, nameofdog, plasticbag) values 
                                                    (@post, @imagefile, @id, @date_published, @date, @age, @breed, @nameofdog, @plasticbag)";

                        if (File.Exists(this.txt_PhotoPath.Text))
                        {
                            byte[] binary = File.ReadAllBytes(this.txt_PhotoPath.Text);
                            cmd.Parameters.Add(new MySqlParameter("imagefile", binary));
                        }
                        else
                            cmd.Parameters.Add(new MySqlParameter("imagefile", null));

                        cmd.Parameters.Add(new MySqlParameter("post", this.viewModel.Post));
                        cmd.Parameters.Add(new MySqlParameter("id", Common.UserInfo.Id));
                        cmd.Parameters.Add(new MySqlParameter("date_published", DateTime.Now));
                        cmd.Parameters.Add(new MySqlParameter("date", new DateTime(this.viewModel.Selectyear.Value, this.viewModel.Selectmonth.Value, this.viewModel.Selectday.Value)));
                        cmd.Parameters.Add(new MySqlParameter("age", this.viewModel.DogAge));
                        cmd.Parameters.Add(new MySqlParameter("breed", this.viewModel.Breed));
                        cmd.Parameters.Add(new MySqlParameter("nameofdog", this.viewModel.DogName));
                        cmd.Parameters.Add(new MySqlParameter("plasticbag", Convert.ToInt32(plasticBag)));

                        if (cmd.ExecuteNonQuery() > 0)
                            MessageBox.Show("등록되었습니다.");
                        else
                            throw new Exception("등록오류");
                    }

                    conn.Close();
                }

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

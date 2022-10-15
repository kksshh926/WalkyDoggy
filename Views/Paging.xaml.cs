using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Paging.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Paging : Page
    {
        private List<BoardItem> boardList;

        private PagingViewModel viewModel = new PagingViewModel();

        public Paging()
        {
            InitializeComponent();

            this.DataContext = this.viewModel;

        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Views/Post.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        private void membersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Uri uri = new Uri("/Views/Chat.xaml", UriKind.Relative);
            //NavigationService.Navigate(uri);
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var dt = (sender as Calendar).SelectedDate;

            this.boardList = new List<BoardItem>();
            var cnt = 0;

            try
            {
                using (var conn = new MySqlConnection(Common.ConnectionString))
                {
                    conn.Open();

                    using (var cmb = conn.CreateCommand())
                    {
                        cmb.CommandText = $@"select * 
                                             from BOARD 
                                             where date between str_to_date('{dt.Value.ToString("yyyy-MM-dd 00:00:00")}', '%Y-%m-%d %H:%i:%f') and str_to_date('{dt.Value.ToString("yyyy-MM-dd 23:59:59")}', '%Y-%m-%d %H:%i:%f')";

                        using (var read = cmb.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                cnt++;

                                this.boardList.Add(new BoardItem
                                {
                                    Number = cnt,
                                    Post = read["post"].ToString(),
                                    imageFile = (byte[])read["imageFile"],
                                    Id = read["id"].ToString(),
                                    Date_Published = Convert.ToDateTime(read["date_published"]),
                                    Date = Convert.ToDateTime(read["date"]),
                                    Age = read["age"].ToString(),
                                    Breed = read["breed"].ToString(),
                                    NameOfDog = read["nameOfDog"].ToString(),
                                    PlasticBag = read["plasticBag"].ToString()
                                });
                            }

                            read.Close();
                        }
                    }
                    
                    conn.Close();
                }

            }catch(Exception ex)
            {

            }
            finally
            {
                this.SetBoardList();
            }
        }

        private void SetBoardList()
        {
            try
            {
                if (string.IsNullOrEmpty(this.viewModel.SearchText))
                    this.viewModel.BoardList = this.boardList;
                else
                {
                    var searchText = this.viewModel.SearchText.Trim().Replace(" ", "").ToLower();
                    this.viewModel.BoardList = this.boardList.Where(x => x.Breed.Trim().Replace(" ", "").ToLower().Contains(searchText)).ToList();
                }
                    

            }catch(Exception e)
            {

            }
        }

        private void txt_SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetBoardList();
        }
    }
}

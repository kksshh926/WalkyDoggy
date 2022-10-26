using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using Walkydoggy.Views;

namespace Walkydoggy.View
{
    /// <summary>
    /// Paging.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Paging : Page
    {
        //private DateTime currDate { get; set; }

        //private DateTime CurrDate { 
        //    get { return this.currDate; } 
        //    set { 
        //        if (this.currDate != value)
        //        {
        //            this.currDate = value;
        //            this.cal_CurrDate.SelectedDate = value;
        //            this.cal_CurrDate.DisplayDate = value;
        //            this.ChangeCurrDate();
        //            this.LoadBoardList();
        //        }

        //    }
        //}
        private List<BoardItem> boardList;

        private PagingViewModel viewModel = new PagingViewModel();

        private Button[] YearButtons => new Button[] {
            btn_Year2020,
            btn_Year2021,
            btn_Year2022,
            btn_Year2023,
            btn_Year2024,
        };

        private Button[] MonthButtons => new Button[]
        {
            btn_Month1,
            btn_Month2,
            btn_Month3,
            btn_Month4,
            btn_Month5,
            btn_Month6,
            btn_Month7,
            btn_Month8,
            btn_Month9,
            btn_Month10,
            btn_Month11,
            btn_Month12,
        };

        public Paging()
        {
            InitializeComponent();

            this.DataContext = this.viewModel;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.cal_CurrDate.SelectedDate = DateTime.Now;
            }
            catch (Exception ex)
            {

            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Views/Post.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        private void membersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            var item = grid.SelectedItem as BoardItem;

            if (item != null)
            {
                var chat = new Chat();
                NavigationService.LoadCompleted += chat.NavigationService_LoadCompleted;

                Uri uri = new Uri("/Views/Chat.xaml", UriKind.Relative);
                NavigationService.Navigate(chat, item);
            }
        }

        /// <summary>
        /// 캘린더 날짜 변경시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var cal = sender as System.Windows.Controls.Calendar;
                var dt = this.cal_CurrDate.SelectedDate.Value;

                if (dt != null)
                {
                    this.ChangeCurrDate();
                    this.LoadBoardList();
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void LoadBoardList()
        {
            try
            {
                var dt = this.cal_CurrDate.SelectedDate.Value;

                this.boardList = new List<BoardItem>();
                var cnt = 0;

                using (var conn = new MySqlConnection(Common.ConnectionString))
                {
                    conn.Open();

                    using (var cmb = conn.CreateCommand())
                    {
                        //게시판 조회 쿼리
                        cmb.CommandText = $@"select * 
                                             from BOARD 
                                             where date between str_to_date('{dt.ToString("yyyy-MM-dd 00:00:00")}', '%Y-%m-%d %H:%i:%f') and str_to_date('{dt.ToString("yyyy-MM-dd 23:59:59")}', '%Y-%m-%d %H:%i:%f')";

                        using (var read = cmb.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                cnt++;

                                //전역변수에 해당날짜 게시물 수집
                                this.boardList.Add(new BoardItem
                                {
                                    Number = cnt,
                                    Post = read["post"].ToString(),
                                    imageFile = read["imageFile"] == DBNull.Value ? null : (byte[])read["imageFile"],
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
                if (string.IsNullOrEmpty(this.viewModel.SearchText))
                    this.viewModel.BoardList = this.boardList;
                else
                {
                    //띄어쓰기와 영대소문자 소문자로 통일하여 Lambda식으로 정리
                    var searchText = this.viewModel.SearchText.Trim().Replace(" ", "").ToLower();
                    this.viewModel.BoardList = this.boardList.Where(x => x.Breed.Trim().Replace(" ", "").ToLower().Contains(searchText)).ToList();
                }


            }
            catch (Exception e)
            {

            }
        }

        private void txt_SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetBoardList();
        }

        private void InitCurrDate()
        {
            try
            {
                foreach (var btn in this.YearButtons)
                    btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBABABA"));

                foreach (var btn in this.MonthButtons)
                    btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBABABA"));

            }
            catch (Exception e)
            {

            }
        }

        private void ChangeCurrDate()
        {
            try
            {
                this.InitCurrDate();

                Color col = new Color();
                switch (this.cal_CurrDate.SelectedDate.Value.Year)
                {
                    case 2020:
                        this.btn_Year2020.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 2021:
                        this.btn_Year2021.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 2022:
                        this.btn_Year2022.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 2023:
                        this.btn_Year2023.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 2024:
                        this.btn_Year2024.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                }

                switch (this.cal_CurrDate.SelectedDate.Value.Month)
                {
                    case 1:
                        this.btn_Month1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 2:
                        this.btn_Month2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 3:
                        this.btn_Month3.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 4:
                        this.btn_Month4.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 5:
                        this.btn_Month5.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 6:
                        this.btn_Month6.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 7:
                        this.btn_Month7.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 8:
                        this.btn_Month8.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 9:
                        this.btn_Month9.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 10:
                        this.btn_Month10.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 11:
                        this.btn_Month11.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                    case 12:
                        this.btn_Month12.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7012"));
                        break;
                }

                this.txtCalendarMonth.Text = this.cal_CurrDate.SelectedDate.Value.ToString("MMMM", CultureInfo.InvariantCulture);
                this.txtCurrMonth.Text = this.cal_CurrDate.SelectedDate.Value.ToString("MMMM", CultureInfo.InvariantCulture);
                this.txtCurrDay.Text = this.cal_CurrDate.SelectedDate.Value.Day.ToString();
                this.txtCurrDate.Text = this.cal_CurrDate.SelectedDate.Value.ToString("dddd", CultureInfo.InvariantCulture);

            }
            catch (Exception e)
            {

            }
        }

        private void YearButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                var year = Convert.ToInt32(btn.Content.ToString());

                this.cal_CurrDate.SelectedDate = new DateTime(year, 1, 1);
                this.cal_CurrDate.DisplayDate = this.cal_CurrDate.SelectedDate.Value;

            }
            catch (Exception ex)
            {

            }
        }

        private void MonthButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                var month = Convert.ToInt32(btn.Content.ToString());

                this.cal_CurrDate.SelectedDate = new DateTime(this.cal_CurrDate.SelectedDate.Value.Year, month, 1);
                this.cal_CurrDate.DisplayDate = this.cal_CurrDate.SelectedDate.Value;
            }
            catch (Exception ex)
            {

            }
        }


    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;
using uPLibrary.Networking.M2Mqtt.Messages;
using Walkydoggy.Models;
using Walkydoggy.ViewModels;

namespace Walkydoggy.Views
{
    /// <summary>
    /// Chat.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Chat : Page
    {
        private PostViewModel viewModel = new PostViewModel();

        private string publisher_Id = string.Empty;

        public Chat()
        {
            InitializeComponent();

            this.DataContext = this.viewModel;
            this.InitTime();
        }

        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.LoadCompleted += this.NavigationService_LoadCompleted;
        }

        public void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                var req = e.ExtraData as BoardItem;

                if (req != null)
                {
                    //내자신의 글일떄는 채팅 비활성
                    if (req.Id.Equals(Common.UserInfo.Id))
                        this.btnStartChat.Visibility = Visibility.Hidden;

                    //게시자 지정
                    this.publisher_Id = req.Id;

                    this.viewModel.Selectyear = req.Date.HasValue ? req.Date.Value.Year : null;
                    this.viewModel.Selectmonth = req.Date.HasValue ? req.Date.Value.Month : null;
                    this.viewModel.Selectday = req.Date.HasValue ? req.Date.Value.Day : null;
                    this.viewModel.Breed = req.Breed;
                    this.viewModel.DogAge = req.Age;
                    this.viewModel.DogName = req.NameOfDog;
                    this.viewModel.Post = req.Post;

                    this.txt_PhotoPath.Source = this.ConvertToImage(req.imageFile);

                    if (Convert.ToInt32(req.PlasticBag).Equals(0))
                        this.toilet_bag_yes.IsChecked = true;
                    else
                        this.toilet_bag_no.IsChecked = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (NavigationService != null)
                    NavigationService.LoadCompleted -= NavigationService_LoadCompleted;
            }
        }

        public BitmapImage ConvertToImage(byte[] buffer)
        {
            MemoryStream stream = new MemoryStream(buffer);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
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

        /// <summary>
        /// 월이 선택되면 해당 월에 마지막일자 계산
        /// </summary>
        private void RefreshDay()
        {
            List<int> days = new List<int>();
            var now = DateTime.Now;

            try
            {
                //년 월이 선택 되어있을떄만 작동하게 예외처리
                if (this.viewModel.Selectyear != null && this.viewModel.Selectmonth != null)
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

        private void cmb_Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //페이지가 로드 됬을떄만 작동하게 예외처리
            if (this.IsLoaded)
                this.RefreshDay();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //채팅창 오픈
            var chattingForm = new Chatting(this.publisher_Id);
            chattingForm.Show();
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using uPLibrary.Networking.M2Mqtt.Messages;
using Walkydoggy.Models;
using Walkydoggy.Views;

namespace Walkydoggy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private List<string> chatting_ids = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            //메시지큐 수신 이벤트 추가
            MyMqtt.client.MqttMsgPublishReceived += this.ReceiveMqttMessage;
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
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReceiveMqttMessage(object sender, EventArgs e)
        {
            //수신받은 메시지큐 수신
            var json = Encoding.UTF8.GetString(((MqttMsgPublishEventArgs)e).Message);
            //json to object 변환
            var receive_data = JsonConvert.DeserializeObject<MqttMessage>(json);
            var message = receive_data.message;
            var pub_name = receive_data.pup_name;
            bool pop = true;

            this.ThreadSafeInvoke(() => {
                //모든 열려있는 WIndow를 검색해서 
                foreach (var form in Application.Current.Windows)
                {
                    var window = form as Window;
                    //이미 열려있는 대화창이면 더이상 팝업되지않게 방지하기위함
                    if (window != null && window.Tag != null && window.Tag.ToString().Equals(pub_name))
                    {
                        pop = false;
                        break;
                    }
                }

                //채팅중이지 않은 사람만 팝업
                if (pop)
                {
                    var chattingForm = new Chatting(pub_name, message);
                    chattingForm.Show();
                }

            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MyMqtt.client.MqttMsgPublishReceived -= this.ReceiveMqttMessage;
        }
    }
}

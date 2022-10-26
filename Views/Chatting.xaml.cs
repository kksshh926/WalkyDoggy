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
using System.Windows.Shapes;
using uPLibrary.Networking.M2Mqtt.Messages;
using Walkydoggy.Models;
using Walkydoggy.ViewModels;

namespace Walkydoggy.Views
{
    /// <summary>
    /// Chatting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Chatting : Window
    {
        private ChattingViewModel viewModel = new ChattingViewModel();
        public string receive_id = string.Empty;

        public Chatting(string receive_id, string? start_message = null)
        {
            InitializeComponent();

            this.DataContext = this.viewModel;
            //대화대상자 지정
            this.receive_id = receive_id;
            //각 채팅창에 대화 대상자 Tag 지정
            this.Tag = receive_id;
            //메시지큐 수신 이벤트 추가
            MyMqtt.MqttMsgPublishReceivedHandler += this.ReceiveMqttMessage;
            //대화창 타이틀 지정
            this.Title =$@"{receive_id}님과의 대화";
            //시작 메시지 설정
            if (start_message != null)
                this.viewModel.Messages.Add(new Message() { Name = receive_id, Content = start_message });
        }

        private void tbContent_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //엔터키를 눌르면 전송
                if (e.Key.Equals(Key.Enter))
                {
                    this.SendToMessage();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.SendToMessage();
        }

        /// <summary>
        /// 메시지큐 발신
        /// </summary>
        private void SendToMessage()
        {
            try
            {
                var content = this.tbContent.Text;

                //메시지큐에 보낼 객체 생성
                MqttMessage message = new MqttMessage
                {
                    pup_type = Models.Enum.MqttPublishType.Chat,
                    message = content,
                    pup_name =  Common.UserInfo.Id
                };

                //Nuget pakage에서 Nutonsoft Json 설치후 Json 가공을 위해 사용
                var json = JsonConvert.SerializeObject(message);

                //채팅 대상에게 발송
                var result = MyMqtt.client.Publish(MyMqtt.CreateTopic(this.receive_id), Encoding.UTF8.GetBytes(json));

                //채팅 내역 추가
                this.ThreadSafeInvoke(() =>
                {
                    this.viewModel.Messages.Add(new Message { Name = Common.UserInfo.Id, Content = content });
                });

                //입력란 비움
                this.tbContent.Text = string.Empty;
            }
            catch (Exception ex)
            {

            }
        }

        private void ReceiveMqttMessage(object sender, EventArgs e)
        {
            //구독중인 토픽에 메시지가 들어왔을떄 json 파싱 
            var json = Encoding.UTF8.GetString(((MqttMsgPublishEventArgs)e).Message);
            var receive_data = JsonConvert.DeserializeObject<MqttMessage>(json);
            var message = receive_data.message;
            var pub_name = receive_data.pup_name;

            this.ThreadSafeInvoke(() =>
            {
                //채팅 리스트 추가
                this.viewModel.Messages.Add(new Message { Name = pub_name, Content = message });

            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //이벤트 다시 회수 (중요)
            MyMqtt.MqttMsgPublishReceivedHandler -= this.ReceiveMqttMessage;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Walkydoggy.ViewModels;
using static Walkydoggy.Models.Enum;

namespace Walkydoggy.Models
{
    public static class MyMqtt
    {
        // 서브스크립 클라이언트 인스턴스 선언
        public static MqttClient client = new MqttClient(Common.BROKER_IP);

        public static void M2MqttInitializeClient()
        {
            try
            {
                //기존 서브스크립 클라이언트가 연결이 되어있다면 연결 해제
                if (client != null && client.IsConnected)
                    client.Disconnect();

                //로그인 정보 체크
                if (Common.UserInfo == null)
                    throw new Exception("로그인을 진행해주세요.");

                //토픽 새로 입력
                //토픽 요소 생성
                string[] topics = new string[] { Common.UserInfo.Id, /*...등등 유저에 대한 정보로 Topic 을 생성하면 됨*/ };

                List<string> real_topics = new List<string>();

                string topic = string.Empty;

                //구독할 토픽 가공
                for (int i = 0; i < topics.Count(); i++)
                {
                    topic += topics[i];
                    real_topics.Add(string.Format("/{0}/#", topic));
                }

                //Mqtt Client 구독 토픽 설정
                var subscribe = client.Subscribe(real_topics.ToArray(), real_topics.Select(x => MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE).ToArray());
                //Mqtt Client 연결
                var connect = client.Connect(Guid.NewGuid().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"채팅서버와 연결중 오류가 발생했습니다. ({ex.Message})");
            }
            finally
            {

            }
        }

        /// <summary>
        /// Topic 생성 메소드 (id 와 다른 고유정보가있으면 파라미터 추가후 사용가능
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string CreateTopic(string Id)
        {
            return $@"/{Id}/";
        }


        public static void M2Mqtt_DisConnect()
        {
            try
            {
                //구독중인 클라이언트가 연결되어있다면 연결 해제
                if (client != null && client.IsConnected)
                    client.Disconnect();

            }
            catch (Exception ex)
            {

            }

        }
    }

    /// <summary>
    /// 메시지큐 전용 클래스
    /// </summary>
    public class MqttMessage
    {
        public MqttPublishType pup_type { get; set; }

        public string pup_name { get; set; }
        public string message { get; set; }

    }
}

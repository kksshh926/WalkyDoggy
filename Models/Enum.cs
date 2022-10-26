using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkydoggy.Models
{
    public static class Enum
    {
        /// <summary>
        /// 메시지큐 타입 (채팅, 등등,..)
        /// </summary>
        public enum MqttPublishType
        {
            Chat = 0,
        }
    }
}

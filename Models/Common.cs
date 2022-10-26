using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkydoggy.Models
{
    public static class Common
    {
        public static string ConnectionString { get; set; } = "SERVER=kksshh926.cafe24.com;DATABASE=kksshh926;UID=kksshh926;PASSWORD=sojin0713!";

        public static User UserInfo { get; set; } = null;

        /// <summary>
        /// 브로커 접속 IP
        /// </summary>
        public const string BROKER_IP = "127.0.0.1"; //로컬 아이피로
    }
}

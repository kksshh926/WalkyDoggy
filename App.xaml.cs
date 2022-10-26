using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Walkydoggy.Models;

namespace Walkydoggy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //종료시 이벤트
        void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                //Mqtt Disconnect
                MyMqtt.M2Mqtt_DisConnect();
            }catch(Exception ex)
            {

            }
        }
    }
}

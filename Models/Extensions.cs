using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Walkydoggy.Models
{
    public static class Extensions
    {
        /// <summary>
        /// Invoke를 실행하고 종료까지 기다린다.
        /// </summary>
        public static async Task<bool> ThreadSafeInvoke(this object obj, Action action)
        {
            bool rtn = false;
            try
            {
                rtn = await System.Windows.Application.Current.Dispatcher.Invoke<Task<bool>>(() => ThreadSafeInvokeDel(action));
            }
            catch (Exception ex)
            {

            }

            return rtn;
        }
        private static async Task<bool> ThreadSafeInvokeDel(Action action)
        {
            action();

            return true;
        }
    }
}

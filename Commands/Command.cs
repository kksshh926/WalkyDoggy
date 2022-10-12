using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Walkydoggy.Commands
{
    class Command
    {
        private Action<object> _execute;
        private Func<object, bool> _canexecute;


        public Command(Action<object> execute, Func<object, bool> canexecute)
        {
            _execute = execute;
            _canexecute = canexecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true; //화면안켜지는 오류 이거 리턴 트루 안해줘서였음 
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}

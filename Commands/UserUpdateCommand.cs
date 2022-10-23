using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Walkydoggy.ViewModels;
using Walkydoggy.View;

namespace Walkydoggy.Commands
{
    internal class UserUpdateCommand : ICommand
    {
        public UserUpdateCommand(UserViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        public UserUpdateCommand()
        {}

        private UserViewModel _ViewModel;

        public event System.EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }


        public bool CanExecute(object parameter)
        {
            return _ViewModel.CanUpdate;
        }

        public void Execute(object parameter)
        {
            //_ViewModel.SaveChanges();
        }
    }
}

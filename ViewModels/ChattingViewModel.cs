using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkydoggy.ViewModels
{
    public class ChattingViewModel : ViewModelBase
    {
        private ObservableCollection<Message> messages = new ObservableCollection<Message>();

        private string receive_id = string.Empty;

        public ObservableCollection<Message> Messages
        {
            get { return this.messages; }
            set { SetProperty<ObservableCollection<Message>>(ref this.messages, value); }
        }

        public string Receive_Id
        {
            get { return this.receive_id; }
            set { SetProperty<string>(ref this.receive_id, value); }
        }
    }
}

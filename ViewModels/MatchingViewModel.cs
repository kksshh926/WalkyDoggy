using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walkydoggy.Models;
using Walkydoggy.ViewModels;

namespace Walkydoggy.ViewModels
{
    internal class MatchingViewModel : Notifier
    {

        string Conn = "SERVER=localhost;DATABASE=walkydog;UID=root;PASSWORD=201933043";

        public MatchingViewModel()
        {
            _User = new User("김두부");

        }



        private User _User;


        public User User
        {
            get
            {
                return _User;
            }
        }

        public String Name
        {
            get
            {
                return User.Name;
            }
            set
            {
                User.Name = value;
            }
        }

    }






}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkydoggy.ViewModels
{
    public class PostViewModel : ViewModelBase
    {
        private List<int> whatyear = new List<int>();
        
        private List<int> whatday = new List<int>();

        private List<int> whatmonth = new List<int>();

        private int? selectyear = null;

        private int? selectmonth = null;

        private int? selectday = null;

        private string breed = string.Empty;

        private string dogAge = string.Empty;

        private string dogName = string.Empty;

        private string content = string.Empty;


        public List<int> Whatyear
        {
            get { return whatyear; }
            set { SetProperty<List<int>>(ref whatyear, value); }
        }

        public List<int> Whatmonth
        {
            get { return whatmonth; }
            set { SetProperty<List<int>>(ref whatmonth, value); }
        }

        public List<int> Whatday
        {
            get { return whatday; }
            set { SetProperty<List<int>>(ref whatday, value); }
        }

        public int? Selectyear
        {
            get { return selectyear; }
            set { SetProperty<int?>(ref selectyear, value); }
        }

        public int? Selectmonth
        {
            get { return selectmonth; }
            set { SetProperty<int?>(ref selectmonth, value); }
        }

        public int? Selectday
        {
            get { return selectday; }
            set { SetProperty<int?>(ref selectday, value); }
        }

        public string Breed
        {
            get { return breed; }
            set { SetProperty<string>(ref breed, value); }
        }

        public string DogAge
        {
            get { return dogAge; }
            set { SetProperty<string>(ref dogAge, value); }
        }

        public string DogName
        {
            get { return dogName; }
            set { SetProperty<string>(ref dogName, value); }
        }

        public string Content
        {
            get { return content; }
            set { SetProperty<string>(ref content, value); }
        }
    }
}

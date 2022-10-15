using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkydoggy.ViewModels
{
    public class PagingViewModel : ViewModelBase
    {
        private string searchText;
        private List<BoardItem> boardList;

        public List<BoardItem> BoardList
        {
            get { return this.boardList; }
            set { SetProperty<List<BoardItem>>(ref boardList, value); }
        }

        public string SearchText
        {
            get { return this.searchText; }
            set { SetProperty<string>(ref searchText, value); }
        }
    }


    public class BoardItem
    {
        public int? Number { get; set; } = null;

        public string Post { get; set; } = string.Empty;

        public byte[] imageFile { get; set; } = null;

        public string Id { get; set; } = string.Empty;

        public DateTime? Date_Published { get; set; } = null;

        public DateTime? Date { get; set; } = null;

        public string Age { get; set; } = string.Empty;

        public string Breed { get; set; } = string.Empty;

        public string NameOfDog { get; set; } = string.Empty;

        public string PlasticBag { get; set; } = string.Empty;

    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Woot.Uwp.Models
{
    public sealed class WootFeedViewModel : INotifyPropertyChanged
    {
        public WootFeedViewModel(string name)
        {
            Name = name;
            Deals = new ObservableCollection<WootDeal>();
            StatusText = "Not loaded.";
        }

        public string Name { get; private set; }
        public ObservableCollection<WootDeal> Deals { get; private set; }
        private string statusText;
        public string StatusText
        {
            get { return statusText; }
            set
            {
                if (statusText == value)
                    return;
                statusText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusText"));
            }
        }
        public bool IsLoaded { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

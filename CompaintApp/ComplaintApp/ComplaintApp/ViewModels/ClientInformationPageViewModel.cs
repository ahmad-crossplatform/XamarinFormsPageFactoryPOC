using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintApp.ViewModels
{
    public class ClientInformationPageViewModel : ViewModelBase
    {
        private string _clientAddress;
        private string _clientName;

        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                OnPropertyChanged();
            }
        }

        public string ClientAddress
        {
            get => _clientAddress;
            set
            {
                _clientAddress = value;
                OnPropertyChanged();
            }
        }

        public Command SaveCommand { get; set; }

        private async void Save()
        {
            IsBusy = true;
            await Task.Delay(2000);
            IsBusy = false;
        }
    }
}
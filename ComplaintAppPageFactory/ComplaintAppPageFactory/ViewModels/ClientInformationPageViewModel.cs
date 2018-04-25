using System.Threading.Tasks;
using ComplaintAppPageFactory.Attributes;
using Xamarin.Forms;

namespace ComplaintAppPageFactory.ViewModels
{
    public class ClientInformationPageViewModel : ViewModelBase
    {
        private string _clientAddress;
        private string _clientName;

        public ClientInformationPageViewModel()
        {
            SaveCommand = new Command(Save);
        }

        [Title( "Give a short description"), Text, Editable, Required]
        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                OnPropertyChanged();
            }
        }

        [Title( "Give a short description"), Text, Editable, Required]
        public string ClientAddress
        {
            get => _clientAddress;
            set
            {
                _clientAddress = value;
                OnPropertyChanged();
            }
        }
        [Title("Save")]
        public Command SaveCommand { get; set; }

        private async void Save()
        {
            IsBusy = true;
            await Task.Delay(2000);
            var pagefactory = new PageFactory();
            await pagefactory.ShowAlert("Saved", "Client Saved!", "Ok");
            IsBusy = false;
        }
    }
}
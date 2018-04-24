using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintApp.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private string _password;
        private string _username;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public Command LoginCommand { get; set; }

        public event EventHandler LoggedIn;

        private async void DoLogin()
        {
            IsBusy = true;
            await Task.Delay(1000);
            LoggedIn?.Invoke(this, null);
            IsBusy = false;
        }
    }
}
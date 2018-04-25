using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintAppPageFactory.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private string _password;
        private string _username;

        public LoginPageViewModel()
        {
            LoginCommand = new Command(DoLogin);
        }
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


        private async void DoLogin()
        {
            IsBusy = true;
            await Task.Delay(1000);
            var pageFactory = new PageFactory();
            await pageFactory.NavigateAsync<MainPageViewModel>();
            IsBusy = false;
        }
    }
}
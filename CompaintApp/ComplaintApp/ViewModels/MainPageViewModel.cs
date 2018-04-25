using System;
using Xamarin.Forms;

namespace ComplaintApp.ViewModels
{
    public class MainPageViewModel:ViewModelBase
    {
        public event EventHandler GeneralInformationClicked; 
        public event EventHandler ProductInformationClicked; 
        public event EventHandler ClientInformationClicked; 
        public MainPageViewModel()
        {
            GeneralInformationCommand = new Command(()=> GeneralInformationClicked?.Invoke(this,null));
            ClientInformationCommand = new Command(()=> ClientInformationClicked?.Invoke(this,null));
            ProductInformationCommand = new Command(()=> ProductInformationClicked?.Invoke(this,null));
        }

        public Command GeneralInformationCommand { get; set; }

        public Command ClientInformationCommand { get; set; }

        public Command ProductInformationCommand { get; set; }
      
    }
}
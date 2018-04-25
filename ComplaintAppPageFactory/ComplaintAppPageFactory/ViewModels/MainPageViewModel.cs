using System;
using System.Windows.Input;
using ComplaintAppPageFactory.Attributes;
using Xamarin.Forms;

namespace ComplaintAppPageFactory.ViewModels
{
    [Title("Main Page")]
    public class MainPageViewModel:ViewModelBase
    {

        public MainPageViewModel()
        {
            var pageFactory = new PageFactory();
            GeneralInformationCommand = new Command(async () => await pageFactory.NavigateAsync<GeneralInformationPageViewModel>()); 
            ClientInformationCommand = new Command(async () => await pageFactory.NavigateAsync<ClientInformationPageViewModel>());
            ProductInformationCommand = new Command(async () => await pageFactory.NavigateAsync<ProductInformationPageViewModel>());
        }

        [Title("General Infomation")]
        public Command GeneralInformationCommand { get; set; }
        
        [Title("Product Infomation")]
        public Command ProductInformationCommand { get; set; }

        [Title("Client Infomation")]
        public Command ClientInformationCommand { get; set; }

    }
}
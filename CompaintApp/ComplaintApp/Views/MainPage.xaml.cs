using ComplaintApp.ViewModels;
using ComplaintApp.Views;
using Xamarin.Forms;

namespace ComplaintApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            if (BindingContext is MainPageViewModel viewmodel)
            {
                viewmodel.ClientInformationClicked += async (sender, args) =>
                {
                    await Navigation.PushAsync(new ClientInformationPage()); 
                }; 
                
                viewmodel.ProductInformationClicked += async (sender, args) =>
                {
                    await Navigation.PushAsync(new ProductInformationPage()); 
                };
                
                viewmodel.GeneralInformationClicked += async (sender, args) =>
                {
                    await Navigation.PushAsync(new GeneralInformationPage()); 
                };
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
    }
}
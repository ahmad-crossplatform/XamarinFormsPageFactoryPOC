using ComplaintApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            if (BindingContext is LoginPageViewModel viewmodel)
            {
                viewmodel.LoggedIn += async (sender, args) => { await Navigation.PushAsync(new MainPage());}; 
            }
        }
    }
}
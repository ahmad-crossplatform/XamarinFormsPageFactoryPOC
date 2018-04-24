using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintApp.CustomControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequiredSwitchCell : RequiredCell
	{
		public RequiredSwitchCell ()
		{
			InitializeComponent ();
            Switch.Toggled += OnToggled;
            IsRequiredLabel.IsVisible = false;
        }

        private void OnToggled(object sender, ToggledEventArgs e)
        {
            IsToggled = e.Value; 
        }

       


        public static readonly BindableProperty IsToggledProperty =
            BindableProperty.Create(nameof(IsToggled), typeof(string), typeof(RequiredSwitchCell), "", propertyChanged: OnIsToggledPropertyChanged);

        public bool IsToggled
        {
            get => (bool)GetValue(IsToggledProperty);
            set => SetValue(IsToggledProperty, value);
        }

        private static void OnIsToggledPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var field = (RequiredSwitchCell)bindable;
            field.Switch.IsToggled = bool.Parse(newvalue.ToString());           
        }
    }
}
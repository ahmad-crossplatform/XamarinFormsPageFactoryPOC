using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintAppPageFactory.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequiredLabelCell : RequiredCell
    {
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(RequiredLabelCell),
                propertyChanged: OnTextPropertyChanged);

        public RequiredLabelCell()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private static void OnTextPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var field = (RequiredLabelCell) bindable;
            field.InfoLabel.Text = newvalue.ToString();
            if (string.IsNullOrEmpty(field.InfoLabel.Text) && field.IsRequired)
                field.IsRequiredLabel.IsVisible = true;
            else
                field.IsRequiredLabel.IsVisible = false;
        }
    }
}
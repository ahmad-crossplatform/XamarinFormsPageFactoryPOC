using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintAppPageFactory.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequiredEntryCell : RequiredCell
    {
        
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(RequiredEntryCell), "",
                propertyChanged: OnValuePropertyChanged);

        public RequiredEntryCell()
        {
            InitializeComponent();
            Entry.TextChanged += OnTextChanged;
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == e.OldTextValue) return;
            Text = e.NewTextValue;
        }

        private static void OnValuePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var field = (RequiredEntryCell) bindable;
            field.Entry.Text = newvalue?.ToString();
            if (string.IsNullOrEmpty(field.Entry.Text) && field.IsRequired)
                field.IsRequiredLabel.IsVisible = true;
            else
                field.IsRequiredLabel.IsVisible = false;
        }
    }
}
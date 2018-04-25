using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintApp.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequiredEditorCell : RequiredCell
    {
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(RequiredEditorCell),
                propertyChanged: OnValuePropertyChanged);

        public RequiredEditorCell()
        {
            InitializeComponent();
            Editor.TextChanged += OnTextChanged;
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
            var field = (RequiredEditorCell) bindable;
            field.Editor.Text = newvalue.ToString();
            if (string.IsNullOrEmpty(field.Editor.Text) && field.IsRequired)
                field.IsRequiredLabel.IsVisible = true;
            else
                field.IsRequiredLabel.IsVisible = false;
        }
    }
}
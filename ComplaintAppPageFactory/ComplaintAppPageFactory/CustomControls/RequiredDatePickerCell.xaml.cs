using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintAppPageFactory.CustomControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequiredDatePickerCell : RequiredCell
	{
        public RequiredDatePickerCell()
        {
            InitializeComponent();
            DatePicker.DateSelected += OnDateSelected;
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            if (e.NewDate == e.OldDate)
            {
                return;
            }
            Date = e.NewDate;
        }
 
        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(RequiredDatePickerCell), DateTime.MinValue, propertyChanged: OnDatePropertyChanged);

        public DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }
 
        private static void OnDatePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
      
            var field = (RequiredDatePickerCell)bindable;
            field.DatePicker.Date = newvalue is DateTime time ? time : new DateTime(); 
            if ((field.DatePicker.Date == null || field.DatePicker.Date == default(DateTime))  && field.IsRequired)
            {
                
                field.IsRequiredLabel.IsVisible = true;

            }
            else
            {
                field.IsRequiredLabel.IsVisible = false;
            }
        }
    }
    
    
}
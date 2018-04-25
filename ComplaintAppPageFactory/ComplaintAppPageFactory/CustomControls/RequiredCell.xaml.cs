using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintAppPageFactory.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile), ContentProperty("IsRequiredLabel")]
    public partial class RequiredCell : ViewCell
    {

        public RequiredCell()
        {
            InitializeComponent();
        }

        public StackLayout OuterStackLayout
        {
            get => _outerStackLayout;
            set
            {
                _outerStackLayout = value;
                MainStackLayout.Children.Add(value); 
            }
        }

        public Label IsRequiredLabel => RequiredLabel; 

        #region Command

        /// <summary>Identifies the Command bindable property.</summary>
        /// <remarks />
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command),
            typeof(ICommand), typeof(RequiredCell), null, BindingMode.OneWay, null, (bindable, oldvalue, newvalue) =>
            {
                var requiredCell = (RequiredCell)bindable;
                var command = (ICommand)newvalue;
                if (command == null)
                    return;
                requiredCell.IsEnabled = command.CanExecute(requiredCell.CommandParameter);
                command.CanExecuteChanged += requiredCell.OnCommandCanExecuteChanged;
            }, (bindable, oldvalue, newvalue) =>
            {
                var requiredCell = (RequiredCell)bindable;
                var command = (ICommand)oldvalue;
                if (command == null)
                    return;
                command.CanExecuteChanged -= requiredCell.OnCommandCanExecuteChanged;
            }, null, null);


        /// <summary>Identifies the CommandParameter bindable property.</summary>
        /// <remarks />
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter), typeof(object), typeof(RequiredCell), null, BindingMode.OneWay, null,
            (bindable, oldvalue, newvalue) =>
            {
                var requiredCell = (RequiredCell)bindable;
                if (requiredCell.Command == null)
                    return;
                requiredCell.IsEnabled = requiredCell.Command.CanExecute(newvalue);
            }, null, null, null);



        protected override void OnTapped()
        {
            base.OnTapped();
            var command = Command;
            command?.Execute(CommandParameter);
        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            IsEnabled = Command.CanExecute(CommandParameter);
        }


        /// <summary>Gets or sets the ICommand to be executed when the RequiredCell is tapped. This is a bindable property.</summary>
        /// <value />
        /// <remarks>
        ///     Setting the Command property has a side effect of changing the Enabled property depending on
        ///     ICommand.CanExecute.
        /// </remarks>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>Gets or sets the parameter passed when invoking the Command. This is a bindable property.</summary>
        /// <value />
        /// <remarks />
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        #endregion


        #region Title

        public static readonly BindableProperty TitleProperty =
         BindableProperty.Create(nameof(Title), typeof(string), typeof(RequiredCell),
        propertyChanged: OnTitlePropertyChanged);


        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        #endregion



        public static readonly BindableProperty IsRequiredProperty =
            BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(RequiredCell), false);

        private StackLayout _outerStackLayout;


        public bool IsRequired
        {
            get => (bool) GetValue(IsRequiredProperty);
            set {
                SetValue(IsRequiredProperty, value);
                IsRequiredLabel.IsVisible = value;
            }
        }


        private static void OnTitlePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var field = (RequiredCell) bindable;
            field.TitleLabel.Text = newvalue.ToString();
        }

    }
}
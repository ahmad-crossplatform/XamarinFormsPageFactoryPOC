using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ComplaintAppPageFactory.Attributes;
using ComplaintAppPageFactory.CustomControls;
using Xamarin.Forms;
using EditorAttribute = ComplaintAppPageFactory.Attributes.EditorAttribute; 
namespace ComplaintAppPageFactory
{
    public interface IPageFactory
    {
        Page CreatePage<T>() where T : INotifyPropertyChanged;
        Page CreatePage(Type type);
        Page CreatePage(INotifyPropertyChanged viewModel, Type type);
        Task NavigateAsync(Page nextPage, bool isAnimated = true);
        Task NavigateAsync<TDestination>(bool isAnimated = true) where TDestination : INotifyPropertyChanged;
        Task<bool> ShowAlert(string title, string message, string ok, string cancel = "");
    }

    public class PageFactory : IPageFactory
    {


        public Page CreatePage<T>() where T : INotifyPropertyChanged
        {

            return CreatePage(typeof(T));
        }

        public Page CreatePage(Type type)
        {
            var viewModel = Activator.CreateInstance(type); 
            return CreatePage((INotifyPropertyChanged)viewModel, type);
        }

        public Page CreatePage(INotifyPropertyChanged viewModel, Type type)
        {

            ContentPage page = new ContentPage();
            var tableView = new TableView() { HasUnevenRows = true };
            var tableRoot = new TableRoot();
            var tableSection = new TableSection();

            var stackLayout = new StackLayout();
            page.Content = stackLayout;
            page.BindingContext = viewModel;

            if (type.CustomAttributes.Any(c => c.AttributeType == typeof(TitleAttribute))) // Page Level
            {
                var titleAttribte = type.CustomAttributes.Single(c => c.AttributeType == typeof(TitleAttribute));
                page.Title = titleAttribte.ConstructorArguments[0].Value.ToString();
            }

            foreach (var propertyInfo in type.GetProperties())
            {
                if (!propertyInfo.CustomAttributes.Any())
                {
                    continue; 
                }
                var isRequired = propertyInfo.CustomAttributes.Any(c => c.AttributeType == typeof(RequiredAttribute));
                RequiredCell child = null;
                var title = "";

                // Title 
                if (propertyInfo.CustomAttributes.Any(c => c.AttributeType == typeof(TitleAttribute)))
                {
                    var titleAttribte = propertyInfo.CustomAttributes.Single(c => c.AttributeType == typeof(TitleAttribute));
                    title = titleAttribte.ConstructorArguments[0].Value.ToString();
                }

                // Text
                if (propertyInfo.CustomAttributes.Any(c => c.AttributeType == typeof(TextAttribute)))
                {

                    if (propertyInfo.CustomAttributes.Any(c => c.AttributeType == typeof(EditableAttribute)))
                    {
                        child = new RequiredEntryCell() { Title = title, IsRequired = isRequired };
                        child.SetBinding(RequiredEntryCell.TextProperty, propertyInfo.Name, BindingMode.TwoWay);

                        if (propertyInfo.CustomAttributes.Any(c => c.AttributeType == typeof(EditorAttribute)))
                        {
                            child = new RequiredEditorCell() { Title = title, IsRequired = isRequired };
                            child.SetBinding(RequiredEditorCell.TextProperty, propertyInfo.Name, BindingMode.TwoWay);
                        }
                    }
                    else  // Create a label 
                    {
                        child = new RequiredLabelCell() { Title = title, IsRequired = isRequired };
                        child.SetBinding(RequiredLabelCell.TextProperty, propertyInfo.Name, BindingMode.TwoWay);

                    }
                }

                // Create switch cell  
                if (propertyInfo.CustomAttributes.Any(c => c.AttributeType == typeof(SwitchAttribute)))
                {
                    child = new RequiredSwitchCell() { Title = title };
                    child.SetBinding(RequiredSwitchCell.IsToggledProperty, propertyInfo.Name, BindingMode.TwoWay);
                }

                // Create a date picker 
                if (propertyInfo.CustomAttributes.Any(c => c.AttributeType == typeof(DateAttribute)))
                {

                    child = new RequiredDatePickerCell() { Title = title };
      
                    child.SetBinding(RequiredDatePickerCell.DateProperty, propertyInfo.Name, BindingMode.TwoWay);
                }

                
                if (propertyInfo.CustomAttributes.Any(c => c.AttributeType == typeof(CommandAttribute)))
                {
                    var commandAttribute = propertyInfo.CustomAttributes.Single(c => c.AttributeType == typeof(CommandAttribute));
                    var commandName = commandAttribute.ConstructorArguments[0].Value.ToString();
                    var command = (ICommand) type.GetProperties().Single(p => p.Name == commandName).GetValue(viewModel);
                    if (command == null)
                    {
                        continue;
                    }

                    if (child != null) child.Command = command;
                }


                //Add the button
                if (propertyInfo.PropertyType == typeof(ICommand) || propertyInfo.PropertyType == typeof(Command))
                {
                    var command = (ICommand)propertyInfo.GetValue(page.BindingContext);
                    var button = new Button
                    {
                        Text = title,
                        Command = command
                    };
                    child = new RequiredCell()
                    {
                         
                        View = button
                    };
                }
                if (child == null)
                {
                    continue;
                }
                tableSection.Add(child);
            }
            tableRoot.Add(tableSection);
            tableView.Root = tableRoot;
            stackLayout.Children.Add(tableView);
            return page;
        }

        public async Task NavigateAsync<TDestination>(bool isAnimated = true) where TDestination : INotifyPropertyChanged
        {

            var nextPage = CreatePage<TDestination>();
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.CurrentPage.Navigation.PushAsync(nextPage, isAnimated);
            }
        }

        public async Task NavigateAsync(Page nextPage, bool isAnimated = true) 
        {

            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.CurrentPage.Navigation.PushAsync(nextPage, isAnimated);
            }
        }

        public async Task<bool> ShowAlert(string title, string message, string ok, string cancel ="")
        {
            if (!(Application.Current.MainPage is NavigationPage navigationPage)) return false;
            if (!string.IsNullOrEmpty(cancel))
            {
                return await navigationPage.CurrentPage.DisplayAlert(title, message, ok, cancel);

            }
            return await navigationPage.CurrentPage.DisplayAlert(title, message, null, ok);
        }
    }
}
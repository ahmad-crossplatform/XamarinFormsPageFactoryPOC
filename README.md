# POC: Xamarin.Forms Page Factory Concept
## Intro

The Page factory can be used for non-commercial Apps which are data oriented to make the process of building an app quicker and more fun. It produces the pages on the fly without you have to design them in advance. To create a page the Page factory takes the designated viewmodel and based on the attributes the properties are tagged with 

## Background

We have got an assignment to make a complaint app. It has a web version, which kind of long with some questions followed by other questions. To make easier for the user , the fields are grouped by relevence. 
![alt text](https://i.imgur.com/FzHozrp.png "a sample of how big can the forms be")

Unlike the web where space is not an issue, space is a serious issue when it comes to mobile apps , after all how big can a mobile screen be . Therefore to overcome this issue we decided to create a page for every group of questions, For example questions  regarding the general information would be in a `GeneralInformationPage` ,  information about product can be in a `ProductInformationPage` and so on. 
In our specific project that means we need to create more than 15 pages which are resposible to fill data of more than 15 grouped information about the complaint. At that piont the idea of having a page factory came to light. 


## PageFactory Concepts
- **Page Factory**
This is the class where it takes viewmodels, produces pages based on the properties and their attributes, and bind the pages to their viewmodels . 
The factory can also used for navigation and it keeps track of the pages. 

``Note: It is important to have one instance of Pagefactory that wil keep track on Pages``
- **Produced Pages** 
Pages which are produced by the Page Factory on the run time.

- **ViewModels**
View models are the main source of information for a page factory in view model , you can decide what properites to be shown or not , how they are  going to be showen (e.g. is it required) , and in what order are they shown.

- **Attributes**
The attributes are custom attributes made by the developer . 
The page factory will look for attributes decorating the properties in the viewmodel to decide how the properties will be represented . 
For example, having a ``Required`` attribute on the property would direct the `PageFactory`  to have the required label activated on the field.
```C#
 public class RequiredAttribute : Attribute
 {
 }
```
If the property has no attributes then the `PageFactory` should ignore it . 

- **Reusable Controls** 
If you have special controls that can be reused it is recommended to have them designed before applying the page factory approach. For example if you have a control that shows the required validation error , then this can be reused easily . 

## Applying the `PageFactory` concept 
Have a look at our repository and you will see there are two Projects ``ComplaintApp`` and ``ComplaintAppPageFactory`` . 

As you might guessed they are the same application but the second one applies the PageFactory concept 

- **Finding the pages which have the same pattern** 
  if we look closely we can find that `MainPage`, `GeneralInformationPage`, `ProductInformationPage` and `ClientInformationPage` have the same pattern; they are pages that has a table view with different cells to represent the data. Some of these cells are required and some are not . They also use custom controls to present the data.  

- **Creating attributes to decorate the viewmodel**
  Now you need to create some custom attributes to decide how each property should be presented , so far we have the following attributes and of course there should be more to cover more cases 

  | Attribute         | Description                                                  |
  | :---------------- | ------------------------------------------------------------ |
  | SwitchAttribute   | The element is a switch                                      |
  | CommandAttribute  | The element has a Command Property                           |
  | DateAttribute     | The element is a DatePicker                                  |
  | TextAttribute     | The element is a label unless the property has `EditableAttribute` |
  | EditableAttribute | The element is `Entry` and it requires `Text` attribute.     |
  | EditorAttribute   | The element is `Editor` and it requires `Text` and `Editable` attributes. |
  | TitleAttribute    | The title of the element and it can be applied on the page level and the button by decorating the class of the VM and the command property |
  | RequiredAttribute | This will enable the required functionality of the element.  |


- **Decorating the View-Models with the attributes** 
  Now we can add the attributes to the properties of each View-model of the pages we . 

  The `Title` attribute can be applied on the class level and that will give a title to the page. If we apply the title attribute to the command level, the page factory with crate a button and give it the title. 

- **Create your page factory**

  Here is our implementation for the page factory 
```C#
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
                if (propertyInfo.PropertyType == typeof(ICommand)|| propertyInfo.PropertyType == typeof(Command) )
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
```
- - For the sake of having a POC the Page Factory produces only ContentPage 
  - To iterate between properties in the viewmodel , Reflection is used. 
  - The checking order can be very important. In this case , we check first if it is a text, then editable then. 
  - `PageFactory` does not only creates pages, but also handles Alerts and Navigation so that they could be called from the view-model . 
  - If you change the order of the properties in the viewmodel then order of the elements in the page will change accordingly.
  - Should the `PageFactory` be a static class? 


- **Apply the PageFactory  concept on the view models.** 

You can start by removing the event handlers which the code behind listens to so it can navigate, and use the navigation methods given by the page factory. 
*Look at the changes we did in `MainPageViewModel`*

- **Remove the views which we targeted earlier**

   So this case   `MainPage`, `GeneralInformationPage`, `ProductInformationPage` and `ClientInformationPage` are removed. 

-  **Give it a go**


## Pros and Cons
### Pros
- **Code Reduction**
If you have multiple pages that can be produced with the Pagefactory then you will basically remove view code for these pages . 

- **Flexibility**
The flexibility gets higher, you can change a feild on a page from one type to another type only by change the decorating attributes , you can change the order of the fields but just changing the order of the properties.  Adding new feilds is easier and quicker. 

- **More focus on logic**
When you get to focus only on the viewmodel , the business logic will get more focus by the developer than if there was a view presenting the view model , where the developer should make sure that  the bindings are correct and if everything is in the right order 

- **Fewer human errors**
Of course when our pages are generated automatically it means that less human interaction is required and thus fewer errors from the developers . 


### Cons 
- **Complexity of the `PageFactory` Class**
More attributes created means more work has to be done in the ``PageFactory`` class where it has to recognize the new attributes and takes the right decision. 
This issue can be addressed by finding a good pattern to write the ``PageFactory`` class. 

- **Not suitable for small apps** 
This concept probably will not be the best choice if the app has few pages or there is no common pattern among them. 
## FAQ
- **When is this method recommended to be used?**
  le apps with several pages which has the same purpose, can be used in Industrial apps, or apps for questionnaires, or any data entry apps that that beauty and interactivity are not the main focus .   

- **When is this method *Not* recommended to be used?**
  Apps with few number of pages , apps with very sophisticated design and styles, or Apps with pages that are totally different from each other and not many similar pages.

- **How mature is this code?**
  Not at all , this is just a POC to show how the PageFactory works . 

- **Can we integrate it with MVVM framework like Prism?**
  Yes. There is already a poc that has a PrismApp where the PageFactory is used. 
  Check [PageFactoryForPrismRepository](https://github.com/ahmad-crossplatform/POC-XF-PageFactory-PRISM) 

- **How about the performance?**
  Our first tests in a low end PC showed that  after the app launches, it takes longer time to generate the first produced page than usual. However after that all the pages are produced instantly just like as if they were already written.  There is a slight chance that 

- **How can this concepts be improved?**

  Many things can make the concept better, to start with more attributes, for example attributes for Image,or List attributes, attributes for page type as currently it is only for `ContentPage`.

  Another improvement can be to have PageFactory in its own nugetPackage . 

  We can think of the custom controls , should they be part of the PageFactory Nuget ? or shall we have a mechanism to connect the Attributes with the visual elements in a dynamic way somehow ? 

- **What is the highest ambition for the PageFactory Approach?**
  To have a standard page factory component, and a view-less  M-V-VM pattern.  


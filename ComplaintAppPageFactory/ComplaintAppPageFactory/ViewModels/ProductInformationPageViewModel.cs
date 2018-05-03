using System;
using ComplaintAppPageFactory.Attributes;

namespace ComplaintAppPageFactory.ViewModels
{
    [Title("Product info")]
    public class ProductInformationPageViewModel : ViewModelBase
    {
        private DateTime _productExpiryDate;

        private string _productModel;
        private string _productName;

        [ Entry, Title("Product Name"), Required]
        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }
        
        [ Entry, Title("Product Model")]
        public string ProductModel
        {
            get => _productModel;
            set
            {
                _productModel = value;
                OnPropertyChanged();
            }
        }

        [Date, Title("Expiry Date"), Required]
        public DateTime ProductExpiryDate
        {
            get => _productExpiryDate;
            set
            {
                _productExpiryDate = value;
                OnPropertyChanged();
            }
        }
    }
}
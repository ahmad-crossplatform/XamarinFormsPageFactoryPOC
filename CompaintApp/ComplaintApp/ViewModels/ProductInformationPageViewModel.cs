using System;

namespace ComplaintApp.ViewModels
{
    public class ProductInformationPageViewModel : ViewModelBase
    {
        private DateTime _productExpiryDate;

        private string _productModel;
        private string _productName;

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }

        public string ProductModel
        {
            get => _productModel;
            set
            {
                _productModel = value;
                OnPropertyChanged();
            }
        }

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
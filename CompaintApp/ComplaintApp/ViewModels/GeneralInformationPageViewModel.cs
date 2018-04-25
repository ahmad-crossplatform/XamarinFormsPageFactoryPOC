using System;

namespace ComplaintApp.ViewModels
{
    public class GeneralInformationPageViewModel : ViewModelBase
    {
        private string _longDescription;
        private DateTime _reportTime;
        private string _shortDescription;

        public DateTime ReportTime
        {
            get => _reportTime;
            set
            {
                _reportTime = value;
                OnPropertyChanged();
            }
        }

        public string ShortDescription
        {
            get => _shortDescription;
            set
            {
                _shortDescription = value;
                OnPropertyChanged();
            }
        }

        public string LongDescription
        {
            get => _longDescription;
            set
            {
                _longDescription = value;
                OnPropertyChanged();
            }
        }
    }
}
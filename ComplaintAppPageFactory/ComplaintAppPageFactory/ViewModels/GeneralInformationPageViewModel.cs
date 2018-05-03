using System;
using ComplaintAppPageFactory.Attributes;

namespace ComplaintAppPageFactory.ViewModels
{
    [Title("General Information")]
    public class GeneralInformationPageViewModel : ViewModelBase
    {
        private string _longDescription;
        private DateTime _reportTime;
        private string _shortDescription;
        [Title("Reported Time"), Date, Required]
        public DateTime ReportTime
        {
            get => _reportTime;
            set
            {
                _reportTime = value;
                OnPropertyChanged();
            }
        }

        [Title( "Give a short description"),  Entry, Required]
        public string ShortDescription
        {
            get => _shortDescription;
            set
            {
                _shortDescription = value;
                OnPropertyChanged();
            }
        }
        [Title( "Describe what happened in details"), LongText, Required]
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
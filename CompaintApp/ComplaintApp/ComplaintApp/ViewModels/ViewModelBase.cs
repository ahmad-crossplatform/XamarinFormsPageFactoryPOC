using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ComplaintApp.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isBusy;

        public string Title { get; set; }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
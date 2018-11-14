using System;
using System.ComponentModel;

namespace WorkStudy.ViewModels
{
    public class AddActivitiesViewModel : INotifyPropertyChanged
    {
        public AddActivitiesViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

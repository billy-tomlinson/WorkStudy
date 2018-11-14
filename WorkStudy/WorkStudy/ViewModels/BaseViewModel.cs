using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public Command SubmitDetails { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel()
        {
            SubmitDetails = new Command(SubmitDetailsAndNavigate);
        }


        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SubmitDetailsAndNavigate(){}
    }
}

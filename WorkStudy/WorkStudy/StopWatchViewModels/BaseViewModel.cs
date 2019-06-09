using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace StopWatch
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Command CloseView { get; set; }
        public Command Override { get; set; }


        public BaseViewModel(string conn = null, string alarmconn = null)
        {
            CloseView = new Command(CloseValidationView);
        }


        bool isInvalid = false;
        public bool IsInvalid
        {
            get { return isInvalid; }
            set
            {
                isInvalid = value;
                OnPropertyChanged();
            }
        }

        int closeColumnSpan = 2;
        public int CloseColumnSpan
        {
            get { return closeColumnSpan; }
            set
            {
                closeColumnSpan = value;
                OnPropertyChanged();
            }
        }

        double opacity = 1;
        public double Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                OnPropertyChanged();
            }
        }

        string invalidText;
        public string InvalidText
        {
            get { return invalidText; }
            set
            {
                invalidText = value;
                OnPropertyChanged();
            }
        }

        bool isOverrideVisible = false;
        public bool IsOverrideVisible
        {
            get { return isOverrideVisible; }
            set
            {
                isOverrideVisible = value;
                OnPropertyChanged();
            }
        }


        bool showOkCancel = false;
        public bool ShowOkCancel
        {
            get { return showOkCancel; }
            set
            {
                showOkCancel = value;
                OnPropertyChanged();
            }
        }

        bool showClose = false;
        public bool ShowClose
        {
            get { return showClose; }
            set
            {
                showClose = value;
                OnPropertyChanged();
            }
        }

        static bool isPageEnabled;
        public bool IsPageEnabled
        {
            get => isPageEnabled;
            set
            {
                isPageEnabled = value;
                OnPropertyChanged();
            }
        }
        private string validationText;
        public string ValidationText
        {
            get => validationText;
            set
            {
                validationText = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void CloseValidationView()
        {
            Opacity = 1;
            IsInvalid = false;
            IsPageEnabled = true;
        }
    }
}

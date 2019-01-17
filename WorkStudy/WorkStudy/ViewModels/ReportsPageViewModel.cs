using System.Threading.Tasks;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class ReportsPageViewModel : BaseViewModel
    {
        public Command SendEmail { get; set; }

        public ReportsPageViewModel()
        {
            ConstructorSetUp();
        }

        private void ConstructorSetUp()
        {
            SendEmail = new Command(SendEmailDetails);
            IsPageVisible = (Utilities.StudyId > 0);
        }

        private bool busy = false;

        public bool IsBusy
        {
            get { return busy; }
            set
            {
                if (busy == value)
                    return;

                busy = value;
                OnPropertyChanged();
            }
        }

        private bool isEnabled = true;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged();
            }
        }

        private async void SendEmailDetails()
        {
            IsBusy = true;
            IsEnabled = false;


            Task emailTask = Task.Run(() => 
            {
                var spreadsheet = new SpreadsheetService().CreateExcelWorkBook();
                Utilities.SendEmail(spreadsheet);
            });

            await emailTask;

            IsEnabled = true;
            IsBusy = false;
        }
    }
}

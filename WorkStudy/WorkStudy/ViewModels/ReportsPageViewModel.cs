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

        private async void SendEmailDetails()
        {
            IsBusy = true;
            IsEnabled = false;
            Opacity = 0.2;
            Task emailTask = Task.Run(() => 
            {
                var spreadsheet = new SpreadsheetService().CreateExcelWorkBook();
                Utilities.SendEmail(spreadsheet);
            });

            await emailTask;

            IsEnabled = true;
            IsBusy = false;
            Opacity = 1;
        }
    }
}

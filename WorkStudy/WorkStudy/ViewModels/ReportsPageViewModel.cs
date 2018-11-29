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

        private void SendEmailDetails()
        {
            
            Activities = Get_Rated_Enabled_Activities();
            var spreadsheet = Utilities.CreateExcelWorkBook(Activities);
            Utilities.SendEmail(spreadsheet);
        }
    }
}

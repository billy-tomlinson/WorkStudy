using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class StudyMenuViewModel : BaseViewModel
    {
        public Command NewStudy { get; set; }
        public Command ExistingStudy { get; set; }
        public Command CompletedStudies { get; set; }

        public StudyMenuViewModel()
        {
            NewStudy = new Command(NewStudyPage);
            ExistingStudy = new Command(ExistingStudyPage);
            CompletedStudies = new Command(CompletedStudiesPage);
        }

        void NewStudyPage()
        {
            App.NavigationPage.Navigation.PushAsync(new NavigationPage(new StudySetUpTabbedPage()));
            //Utilities.Navigate(new StudyDetails());
        }

        void ExistingStudyPage()
        {
            Utilities.Navigate(new PausedStudiesPage(false));
        }

        void CompletedStudiesPage()
        {
            Utilities.Navigate(new CompletedStudiesPage(true));
        }
    }
}

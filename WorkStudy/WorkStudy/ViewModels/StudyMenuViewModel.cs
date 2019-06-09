using TimeStudy.Pages;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class StudyMenuViewModel : BaseViewModel
    {
        public Command NewStudy { get; set; }
        public Command ExistingStudy { get; set; }
        public Command SwitchTimeStudy { get; set; }
        public Command SwitchRAS { get; set; }

        public StudyMenuViewModel()
        {
            NewStudy = new Command(NewStudyPage);
            ExistingStudy = new Command(ExistingStudyPage);
            SwitchTimeStudy = new Command(SwitchTimeStudyMenu);
            SwitchRAS = new Command(SwitchRASMenu);
        }

        void NewStudyPage()
        {
            Utilities.Navigate(new StudyDetailsPage());
        }

        void ExistingStudyPage()
        {
            Utilities.Navigate(new WorkStudy.Pages.ExistingStudiesTabbedPage());
        }

        void SwitchTimeStudyMenu()
        {
            Utilities.SwitchTimeStudyMenuEvent();
        }

        void SwitchRASMenu()
        {
            Utilities.SwitchRASMenuEvent();
        }
    }
}

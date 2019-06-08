using TimeStudy.Pages;
using TimeStudy.Services;
using WorkStudy.Pages;
using Xamarin.Forms;

namespace TimeStudy.ViewModels
{
    public class TimeStudyMenuViewModel : BaseViewModel
    {
        public Command NewStudy { get; set; }
        public Command ExistingStudy { get; set; }
        public Command SwitchTimeStudy { get; set; }
        public Command SwitchRAS { get; set; }

        public TimeStudyMenuViewModel()
        {
            NewStudy = new Command(NewStudyPage);
            ExistingStudy = new Command(ExistingStudyPage);
            SwitchTimeStudy = new Command(SwitchTimeStudyMenu);
            SwitchRAS = new Command(SwitchRASMenu);
        }

        void NewStudyPage()
        {
            Utilities.Navigate(new TimeStudySetUpTabbedPage());
        }

        void ExistingStudyPage()
        {
            Utilities.Navigate(new TimeStudy.Pages.ExistingStudiesTabbedPage()); 
        }

        void SwitchTimeStudyMenu()
        {
            SwitchTimeStudyMenuEvent();
        }

        void SwitchRASMenu()
        {
            SwitchRASMenuEvent();
        }

        private void SwitchTimeStudyMenuEvent()
        {
            var menuPage = new HamburgerMenuPage() { Title = "Main Page", Icon = "hamburger.png" };

            var md = (MasterDetailPage)Application.Current.MainPage;
            md.Master = menuPage;
        }
        private void SwitchRASMenuEvent()
        {
            var menuPage = new MenuPage() { Title = "Main Page", Icon = "hamburger.png" };

            var md = (MasterDetailPage)Application.Current.MainPage;
            md.Master = menuPage;
        }
    }
}

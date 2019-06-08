using TimeStudy.Pages;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public Command SwitchTimeStudy { get; set; }
        public Command SwitchRAS { get; set; }
        public Command SwitchStopWatch { get; set; }

        public HomePageViewModel()
        {
            SwitchTimeStudy = new Command(SwitchTimeStudyMenu);
            SwitchRAS = new Command(SwitchRASMenu);
            SwitchStopWatch = new Command(SwitchStopWatchMenu);
        }

        void SwitchTimeStudyMenu()
        {
            SwitchTimeStudyMenuEvent();
            Utilities.Navigate(new TimeStudyMenuPage());
        }

        void SwitchRASMenu()
        {
            SwitchRASMenuEvent();
            Utilities.Navigate(new StudyMenuPage());
        }

        void SwitchStopWatchMenu()
        {
            SwitchStopWatchMenuEvent();
            Utilities.Navigate(new StopWatch.StopWatchPage());
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

        private void SwitchStopWatchMenuEvent()
        {
            var menuPage = new StopWatch.StopWatchMenuPage() { Title = "Main Page", Icon = "hamburger.png" };

            var md = (MasterDetailPage)Application.Current.MainPage;
            md.Master = menuPage;
        }
    }
}
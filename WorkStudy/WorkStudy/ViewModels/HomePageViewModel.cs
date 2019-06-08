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

        public HomePageViewModel()
        {
            SwitchTimeStudy = new Command(SwitchTimeStudyMenu);
            SwitchRAS = new Command(SwitchRASMenu);
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
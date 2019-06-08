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

        public static NavigationPage NavigationPage { get; private set; }
        public static RootPage RootPage;
        public static bool MenuIsPresented
        {
            get
            {
                return RootPage.IsPresented;
            }
            set
            {
                RootPage.IsPresented = value;
            }
        }

        public StudyMenuViewModel()
        {
            NewStudy = new Command(NewStudyPage);
            ExistingStudy = new Command(ExistingStudyPage);
            SwitchTimeStudy = new Command(SwitchTimeStudyMenu);
            SwitchRAS = new Command(SwitchRASMenu);
        }

        void NewStudyPage()
        {
            Utilities.Navigate(new StudySetUpTabbedPage());
        }

        void ExistingStudyPage()
        {
            //Utilities.Navigate(new ExistingStudiesTabbedPage());
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

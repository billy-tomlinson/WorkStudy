using System.Windows.Input;
using TimeStudy.Pages;
using TimeStudy.Services;
using TimeStudyApp.Pages;
using WorkStudy.Pages;
using Xamarin.Forms;

namespace TimeStudy.ViewModels
{
    public class HamburgerMenuPageViewModel : BaseViewModel
    {
        public ICommand HomePageMenu { get; set; }
        public ICommand ExistingStudies { get; set; }
        public ICommand Reports { get; set; }
        public ICommand CurrentStudy { get; set; }
        public ICommand StudySetUp { get; set; }
        public ICommand CloseApplication { get; set; }
        public ICommand About { get; set; }
        public ICommand CurrentStudyDetails { get; set; }

        public string VersionAndBuild => "version " + Utilities.Version + " build " + Utilities.Build;

        public HamburgerMenuPageViewModel()
        {
            HomePageMenu = new Command(GoHomePageMenu);
            ExistingStudies = new Command(GoExistingStudies);
            Reports = new Command(GoReports);
            CurrentStudy = new Command(GoCurrentStudy);
            StudySetUp = new Command(GoStudySetUp);
            CloseApplication = new Command(CloseApplicationEvent);
            About = new Command(AboutEvent);
            CurrentStudyDetails = new Command(CurrentStudyDetailsEvent);
        }

        void CloseApplicationEvent(object obj)
        {
            DependencyService.Get<WorkStudy.Services.ITerminateApplication>()
                .CloseApplication();
            WorkStudy.App.MenuIsPresented = false;
        }

        void GoHomePageMenu(object obj)
        {
            WorkStudy.Services.Utilities.SwitchHomeMenuEvent();
            Utilities.Navigate(new HomePage());
            WorkStudy.App.MenuIsPresented = false;
        }

        void CurrentStudyDetailsEvent(object obj)
        {
            Utilities.Navigate(new CurrentStudyDetailsPage());
            WorkStudy.App.MenuIsPresented = false;
        }

        void GoCurrentStudy(object obj)
        {
            //if (Utilities.StudyId == 0) return;
            Utilities.Navigate(new TimeStudyMainPageTabbedPage());
            WorkStudy.App.MenuIsPresented = false;
        }

        void GoStudySetUp(object obj)
        {
            Utilities.StudyId = 0;
            Utilities.Navigate(new TimeStudySetUpTabbedPage());
            WorkStudy.App.MenuIsPresented = false;
        }

        void GoExistingStudies(object obj)
        {
            Utilities.Navigate(new TimeStudy.Pages.ExistingStudiesTabbedPage());
            WorkStudy.App.MenuIsPresented = false;
        }

        void GoReports(object obj)
        {
            Utilities.Navigate(new TimeStudyReportsPage());
            WorkStudy.App.MenuIsPresented = false;
        }

        void AboutEvent(object obj)
        {
            Utilities.Navigate(new TimeStudyAboutPage());
            WorkStudy.App.MenuIsPresented = false;
        }
    }
}

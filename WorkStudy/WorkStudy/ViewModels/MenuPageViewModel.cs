using System.Windows.Input;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class MenuPageViewModel : BaseViewModel
    {
        public ICommand HomePageMenu { get; set; }
        public ICommand StopWatch { get; set; }
        public ICommand AddActivities { get; set; }
        public ICommand MergeActivities { get; set; }
        public ICommand AddOperators { get; set; }
        public ICommand ExistingStudies { get; set; }
        public ICommand Reports { get; set; }
        public ICommand CurrentStudy { get; set; }
        public ICommand StudySetUp { get; set; }
        public ICommand AlarmSetUp { get; set; }
        public ICommand CloseApplication { get; set; }
        public ICommand About { get; set; }

        public string VersionAndBuild => "version " + Utilities.Version + " build " + Utilities.Build;

        public MenuPageViewModel()
        {
            HomePageMenu = new Command(GoHomePageMenu);
            StopWatch = new Command(GoStopWatch);
            AddActivities = new Command(GoActivities);
            MergeActivities = new Command(GoMergeActivities);
            AddOperators = new Command(GoOperators);
            ExistingStudies = new Command(GoExistingStudies);
            Reports = new Command(GoReports);
            CurrentStudy = new Command(GoCurrentStudy);
            StudySetUp = new Command(GoStudySetUp);
            AlarmSetUp = new Command(AlarmSetUpEvent);
            CloseApplication = new Command(CloseApplicationEvent);
            About = new Command(AboutEvent);
        }

        void AlarmSetUpEvent(object obj)
        {
            Utilities.Navigate(new AlarmPage());
            App.MenuIsPresented = false;
        }

        void CloseApplicationEvent(object obj)
        {
            AlarmNotificationService.DisableAlarmInDatabase();
            AlarmNotificationService.DisableAlarm();
            DependencyService.Get<ITerminateApplication>()
                .CloseApplication();
            App.MenuIsPresented = false;
        }

        void GoHomePageMenu(object obj)
        {
            Utilities.SwitchHomeMenuEvent();
            Utilities.Navigate(new HomePage());
            App.MenuIsPresented = false;
        }

        void GoStopWatch(object obj)
        {
            Utilities.Navigate(new StopWatch.StopWatchPage());
            App.MenuIsPresented = false;
        }

        void GoCurrentStudy(object obj)
        {
            Utilities.Navigate(new MainPageTabbedPage());
            App.MenuIsPresented = false;
        }

        void GoStudySetUp(object obj)
        {
            Utilities.StudyId = 0;
            Utilities.Navigate(new StudyDetailsPage());
            App.MenuIsPresented = false;
        }

        void GoActivities(object obj)
        {
            Utilities.Navigate(new AddActivitiesPage()); 
            App.MenuIsPresented = false;
        }

        void GoMergeActivities(object obj)
        {
            Utilities.Navigate(new EditActivitiesPage());
            App.MenuIsPresented = false;
        }

        void GoOperators(object obj)
        {
            Utilities.Navigate(new AddOperatorsPage());
            App.MenuIsPresented = false;
        }

        void GoExistingStudies(object obj)
        {
            Utilities.Navigate(new ExistingStudiesTabbedPage());
            App.MenuIsPresented = false;
        }

        void GoReports(object obj)
        {
            Utilities.Navigate(new ReportsPage());
            App.MenuIsPresented = false;
        }

        void AboutEvent(object obj)
        {
            Utilities.Navigate(new AboutPage());
            App.MenuIsPresented = false;
        }
    }
}

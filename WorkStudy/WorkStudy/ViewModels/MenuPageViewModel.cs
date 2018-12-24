using System.Windows.Input;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class MenuPageViewModel : BaseViewModel
    {
        public ICommand StudyMenu { get; set; }
        public ICommand AddActivities { get; set; }
        public ICommand MergeActivities { get; set; }
        public ICommand AddOperators { get; set; }
        public ICommand ExistingStudies { get; set; }
        public ICommand Reports { get; set; }
        public ICommand CurrentStudy { get; set; }
        public ICommand StudySetUp { get; set; }
        public ICommand TurnOffAlarmCommand { get; set; }
        public ICommand AlarmSetUp { get; set; }

        public MenuPageViewModel()
        {
            StudyMenu = new Command(GoStudyMenu);
            AddActivities = new Command(GoActivities);
            MergeActivities = new Command(GoMergeActivities);
            AddOperators = new Command(GoOperators);
            ExistingStudies = new Command(GoExistingStudies);
            Reports = new Command(GoReports);
            CurrentStudy = new Command(GoCurrentStudy);
            StudySetUp = new Command(GoStudySetUp);
            TurnOffAlarmCommand = new Command(TurnOffAlarmEvent);
            AlarmSetUp = new Command(AlarmSetUpEvent);
        }

        void TurnOffAlarmEvent(object obj)
        {
            TurnOffAlarm();
        }

        void AlarmSetUpEvent(object obj)
        {
            Utilities.Navigate(new AlarmPage());
            App.MenuIsPresented = false;
        }

        void GoStudyMenu(object obj)
        {
            Utilities.StudyId = 0;
            Utilities.Navigate(new StudyMenuPage());
            App.MenuIsPresented = false;
        }

        void GoCurrentStudy(object obj)
        {
            Utilities.Navigate(new MainPageTabbedPage());
            App.MenuIsPresented = false;
        }

        void GoStudySetUp(object obj)
        {
            Utilities.Navigate(new StudySetUpTabbedPage());
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
    }
}

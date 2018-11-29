using System.Windows.Input;
using WorkStudy.Pages;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class MenuPageViewModel
    {
        public ICommand StudyMenu { get; set; }
        public ICommand AddActivities { get; set; }
        public ICommand AddOperators { get; set; }
        public ICommand CompletedStudies { get; set; }
        public ICommand PausedStudies { get; set; }
        public ICommand Reports { get; set; }

        public MenuPageViewModel()
        {
            StudyMenu = new Command(GoStudyMenu);
            AddActivities = new Command(GoActivities);
            AddOperators = new Command(GoOperators);
            CompletedStudies = new Command(GoCompletedStudies);
            PausedStudies = new Command(GoPausedStudies);
            Reports = new Command(GoReports);
        }

        void GoStudyMenu(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new StudyMenu());
            App.MenuIsPresented = false;
        }

        void GoActivities(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new AddActivities()); 
            App.MenuIsPresented = false;
        }

        void GoOperators(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new AddOperators());
            App.MenuIsPresented = false;
        }

        void GoCompletedStudies(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new CompletedStudiesPage(true));
            App.MenuIsPresented = false;
        }

        void GoPausedStudies(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new PausedStudiesPage(false));
            App.MenuIsPresented = false;
        }

        void GoReports(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new ReportsPage());
            App.MenuIsPresented = false;
        }

       
    }
}

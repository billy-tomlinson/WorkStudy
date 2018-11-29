using System.Windows.Input;
using WorkStudy.Pages;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class MenuPageViewModel
    {
        public ICommand GoHomeCommand { get; set; }
        public ICommand GoSecondCommand { get; set; }
        public ICommand GoThirdCommand { get; set; }
        public MenuPageViewModel()
        {
            GoHomeCommand = new Command(GoHome);
            GoSecondCommand = new Command(GoSecond);
            GoThirdCommand = new Command(GoThird);
        }

        void GoHome(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new StudyMenu());
            //App.NavigationPage.Navigation.PopToRootAsync();
            App.MenuIsPresented = false;
        }

        void GoSecond(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new AddActivities()); //the content page you wanna load on this click event 
            App.MenuIsPresented = false;
        }

        void GoThird(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new AddOperators());
            App.MenuIsPresented = false;
        }

       
    }
}

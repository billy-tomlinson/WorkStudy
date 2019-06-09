using System.Windows.Input;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;

namespace StopWatch
{
    public class StopWatchMenuPageViewModel : BaseViewModel
    {
        public ICommand CloseApplication { get; set; }
        public ICommand StopWatch { get; set; }
        public ICommand HomePageMenu { get; set; }

        public StopWatchMenuPageViewModel()
        {
            CloseApplication = new Command(CloseApplicationEvent);
            StopWatch = new Command(StopWatchEvent);
            HomePageMenu = new Command(HomePageMenuEvent);
        }

        public string VersionAndBuild => "version " + Utilities.Version + " build " + Utilities.Build;

        void CloseApplicationEvent(object obj)
        {
            DependencyService.Get<ITerminateApplication>()
                .CloseApplication();
            WorkStudy.App.MenuIsPresented = false;
        }

        void StopWatchEvent(object obj)
        {
            Utilities.Navigate(new StopWatchPage());
            WorkStudy.App.MenuIsPresented = false;
        }

        void HomePageMenuEvent(object obj)
        {
            Utilities.SwitchHomeMenuEvent();
            Utilities.Navigate(new HomePage());
            WorkStudy.App.MenuIsPresented = false;
        }
    }
}

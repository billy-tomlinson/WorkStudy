using System.Windows.Input;
using TimeStudy.Pages;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class HomeMenuPageViewModel : BaseViewModel
    {
        public ICommand TimeStudy { get; set; }
        public ICommand StopWatch { get; set; }
        public ICommand ActivitySampling { get; set; }
        public ICommand CloseApplication { get; set; }
        public ICommand About { get; set; }

        public string VersionAndBuild => "version " + Utilities.Version + " build " + Utilities.Build;

        public HomeMenuPageViewModel()
        {
            TimeStudy = new Command(TimeStudyEvent);
            StopWatch = new Command(StopWatchEvent);
            ActivitySampling = new Command(ActivitySamplingEvent);
            CloseApplication = new Command(CloseApplicationEvent);
            About = new Command(AboutEvent);
        }

        void CloseApplicationEvent(object obj)
        {
            AlarmNotificationService.DisableAlarmInDatabase();
            AlarmNotificationService.DisableAlarm();
            DependencyService.Get<ITerminateApplication>()
                .CloseApplication();
            App.MenuIsPresented = false;
        }

        void TimeStudyEvent(object obj)
        {
            Utilities.SwitchTimeStudyMenuEvent();
            Utilities.Navigate(new TimeStudyMenuPage());
            App.MenuIsPresented = false;
        }

        void StopWatchEvent(object obj)
        {
            Utilities.SwitchStopWatchMenuEvent();
            Utilities.Navigate(new StopWatch.StopWatchPage());
            App.MenuIsPresented = false;
        }

        void ActivitySamplingEvent(object obj)
        {
            Utilities.SwitchRASMenuEvent();
            Utilities.Navigate(new StudyMenuPage());
            App.MenuIsPresented = false;
        }

        void AboutEvent(object obj)
        {
            Utilities.Navigate(new AboutPage());
            App.MenuIsPresented = false;
        }
    }
}

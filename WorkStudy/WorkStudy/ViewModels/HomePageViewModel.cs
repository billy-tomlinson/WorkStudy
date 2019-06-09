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
            DisableAlarm();
            Utilities.SwitchTimeStudyMenuEvent();
            Utilities.Navigate(new TimeStudyMenuPage());
        }

        void SwitchRASMenu()
        {
            Utilities.SwitchRASMenuEvent();
            Utilities.Navigate(new StudyMenuPage());
        }

        void SwitchStopWatchMenu()
        {
            DisableAlarm();
            Utilities.SwitchStopWatchMenuEvent();
            Utilities.Navigate(new StopWatch.StopWatchPage());
        }

        private void DisableAlarm()
        {
            AlarmNotificationService.DisableAlarmInDatabase();
            AlarmNotificationService.DisableAlarm();
        }
    }
}
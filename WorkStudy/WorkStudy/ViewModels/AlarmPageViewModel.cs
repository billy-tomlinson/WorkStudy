using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using WorkStudy.Services;

namespace WorkStudy.ViewModels
{
    public class AlarmPageViewModel : BaseViewModel
    {
        public ICommand DisableAlarm { get; set; }
        public ICommand EnableAlarm { get; set; }

        public bool CancelAlarm { get; set; }

        public bool ContinueTimer { get; set; } = true;

        static bool _isIntervalMinutesVisible;
        public bool IsIntervalMinutesVisible
        {
            get => _isIntervalMinutesVisible;
            set
            {
                _isIntervalMinutesVisible = value;
                OnPropertyChanged();
            }
        }

        public AlarmPageViewModel()
        {
            DisableAlarm = new Command(DisableAlarmEvent);
            EnableAlarm = new Command(EnableAlarmEvent);
        }

        void EnableAlarmEvent(object obj)
        {
            int interval = 5; //use this as a temp - will be replaced with a random

            if(CountriesSelectedIndex == 1)
            {
                var success = int.TryParse(IntervalMinutes, out int result);
                if (!success)
                {
                    ValidationText = "Please enter valid minutes less than 99";
                    Opacity = 0.2;
                    IsInvalid = true;
                    return;
                }

                interval = result * 60;
            }
            else
                interval = interval * 60;

            DependencyService.Get<ILocalNotificationService>()
                .LocalNotification("Alert", "Next Observation Round", 0, DateTime.Now, interval);
            AlarmStatus = "Alarm is enabled";
        }

        void DisableAlarmEvent(object obj)
        {
            DependencyService.Get<ILocalNotificationService>()
                .DisableLocalNotification("Alert", "Next Observation Round", 0, DateTime.Now);
            AlarmStatus = "Alarm is disabled";
        }

        int countriesSelectedIndex;
        public int CountriesSelectedIndex
        {
            get => countriesSelectedIndex;
            set
            {
                if (countriesSelectedIndex != value)
                {
                    countriesSelectedIndex = value;
                    OnPropertyChanged();
                    IsIntervalMinutesVisible = countriesSelectedIndex == 1;
                }
            }
        }

        public List<string> IntervalTypes { get; } = new List<string>
        {
            "Random",
            "Interval"
        };
    }
}

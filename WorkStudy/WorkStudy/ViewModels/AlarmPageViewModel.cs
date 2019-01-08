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

        static bool isIntervalMinutesVisible;
        public bool IsIntervalMinutesVisible
        {
            get => !IsRandom;
            set
            {
                isIntervalMinutesVisible = value;
                OnPropertyChanged();
            }
        }

        static Color disabledColour;
        public Color DisabledColour
        {
            get => disabledColour;
            set
            {
                disabledColour = value;
                OnPropertyChanged();
            }
        }

        static bool isRandom;
        public bool IsRandom
        {
            get => isRandom;
            set
            {
                isRandom = value;
                OnPropertyChanged();
                OnPropertyChanged("IsIntervalMinutesVisible");
                Switch_Toggled();
            }
        }


        static string alarmType = "INTERVAL";
        public string AlarmType
        {
            get => alarmType;
            set
            {
                alarmType = value;
                OnPropertyChanged();
            }
        }

        void Switch_Toggled()
        {
            AlarmType = isRandom == false ? "INTERVAL" : "RANDOM";
            DisabledColour = isRandom == false ? Color.White : Color.Gray;
        }

        public AlarmPageViewModel()
        {
            DisableAlarm = new Command(DisableAlarmEvent);
            EnableAlarm = new Command(EnableAlarmEvent);
        }

        void EnableAlarmEvent(object obj)
        {
            int interval = 5; //use this as a temp - will be replaced with a random

            if(!IsRandom)
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
    }
}

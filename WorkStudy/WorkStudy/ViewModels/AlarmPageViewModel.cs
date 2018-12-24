using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AlarmPageViewModel : BaseViewModel
    {
        public ICommand DisableAlarm { get; set; }
        public ICommand EnableAlarm { get; set; }

        static bool isIntervalMinutesVisible;
        public bool IsIntervalMinutesVisible
        {
            get => isIntervalMinutesVisible;
            set
            {
                isIntervalMinutesVisible = value;
                OnPropertyChanged();
            }
        }

        public AlarmPageViewModel()
        {
            DisableAlarm = new Command(DisableAlarmEvent);
            EnableAlarm = new Command(EnableAlarmEvent);
        }

        void DisableAlarmEvent(object obj)
        {
            ContinueTimer = false;
            CancelAlarm = true;
            AlarmStatus = "Alarm is disabled";
        }

        void EnableAlarmEvent(object obj)
        {
            var success = int.TryParse(IntervalMinutes, out int result);
            if (!success)
            {
                ValidationText = "Please enter valid minutes less than 99";
                Opacity = 0.2;
                IsInvalid = true;
                return;
            }
            ContinueTimer = true;
            StartVibrateTimer();
            AlarmStatus = "Alarm is enabled";
        }

        int countriesSelectedIndex;
        public int CountriesSelectedIndex
        {
            get
            {
                return countriesSelectedIndex;
            }
            set
            {
                if (countriesSelectedIndex != value)
                {
                    countriesSelectedIndex = value;
                    OnPropertyChanged();
                    if (countriesSelectedIndex == 1)
                        IsIntervalMinutesVisible = true;
                    else
                        IsIntervalMinutesVisible = false;
                }
            }
        }

        List<string> intervalTypes = new List<string>
        {
            "Random",
            "Interval"   
        };

        public List<string> IntervalTypes => intervalTypes;
    }
}

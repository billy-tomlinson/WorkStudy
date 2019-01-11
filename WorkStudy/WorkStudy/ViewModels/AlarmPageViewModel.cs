using System;
using Xamarin.Forms;
using WorkStudy.Services;
using WorkStudy.Model;

namespace WorkStudy.ViewModels
{
    public class AlarmPageViewModel : BaseViewModel
    {
        const string interval = "INTERVAL";
        const string random = "RANDOM";
        int intervalTime = 1;
        readonly bool pageLoading;

        AlarmDetails alarmDetails;

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
                Switch_Toggled_Type();
                if(!pageLoading)
                {
                    var success = int.TryParse(IntervalMinutes, out int result);
                    IntervalIsValid(success);
                    IsEnabled = false;
                    OnPropertyChanged("IsEnabled");
                    IsPageEnabled = true;
                    OnPropertyChanged("IsPageEnabled");
                    if (value)
                    {
                        IntervalMinutes = null;
                        OnPropertyChanged("IntervalMinutes");
                    }

                }   
            }
        }

        static bool isPageEnabled;
        public bool IsPageEnabled
        {
            get => !IsEnabled;
            set
            {
                isPageEnabled = value;
                OnPropertyChanged();
            }
        }


        static bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged();
                OnPropertyChanged("AlarmType");
                Switch_Toggled_Enabled();
                if(!pageLoading)
                {
                    SaveAlarmDetails();
                    if (value)
                        IsPageEnabled = false;
                        OnPropertyChanged("IsPageEnabled");
                }

            }
        }

        static string alarmType = interval;
        public string AlarmType
        {
            get => alarmType;
            set
            {
                alarmType = value;
                OnPropertyChanged();
            }
        }

        void Switch_Toggled_Type()
        {
            alarmDetails = AlarmRepo.GetItem(1) ?? new AlarmDetails();
            intervalTime = alarmDetails.Interval / 60;
            AlarmType = isRandom == false ? interval : random;
            DisabledColour = isRandom == false ? Color.White : Color.Gray;
        }


        void Switch_Toggled_Enabled()
        {
            alarmDetails = AlarmRepo.GetItem(1) ?? new AlarmDetails();
            intervalTime = alarmDetails.Interval / 60;
            if (IsEnabled)
                AlarmStatus = "ENABLED";
            else
                AlarmStatus = "DISABLED";
        }

        public AlarmPageViewModel()
        {
            pageLoading = true;
            alarmDetails = AlarmRepo.GetItem(1) ?? new AlarmDetails();

            AlarmType = alarmDetails.Type != string.Empty ? alarmDetails.Type : interval;
            IsRandom = alarmDetails.Type != interval;
            IsEnabled = alarmDetails.IsActive;
            IntervalMinutes = (alarmDetails.Interval / 60).ToString();
            pageLoading = false;
        }

        void SaveAlarmDetails()
        {
            if (!IsRandom && IsEnabled)
            {
                var success = int.TryParse(IntervalMinutes, out int result);

                if(!IntervalIsValid(success)) return;

                intervalTime = result * 60;
            }
            else
                intervalTime = intervalTime * 60;

            alarmDetails.Interval = intervalTime;
            alarmDetails.Type = AlarmType;
            alarmDetails.IsActive = IsEnabled;

            AlarmRepo.SaveItem(alarmDetails);

            var service = DependencyService.Get<ILocalNotificationService>();

            if (IsEnabled)
                service.LocalNotification("Alert", "Next Observation Round", 0, DateTime.Now, intervalTime);
            else
                service.DisableLocalNotification("Alert", "Next Observation Round", 0, DateTime.Now);
        }

        private bool IntervalIsValid(bool success)
        {
            if (!IsEnabled) return true;

            if (!success)
            {
                ValidationText = "Please enter valid minutes less than 99";
                Opacity = 0.2;
                IsInvalid = true;
                IsEnabled = false;
                Switch_Toggled_Enabled();
                return false;

            }
            else
                return true;

        }
    }
}

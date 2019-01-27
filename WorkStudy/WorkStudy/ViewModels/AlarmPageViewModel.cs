using WorkStudy.Services;
using WorkStudy.Model;
using System.Linq;

namespace WorkStudy.ViewModels
{
    public class AlarmPageViewModel : BaseViewModel
    {
        const string interval = "CONSTANT";

        int intervalTime = 1;
        readonly bool pageLoading;

        AlarmDetails alarmDetails;

        public bool CancelAlarm { get; set; }

        public ActivitySampleStudy ActivtySample { get; set; }

        public string StudyType { get; set; }

        public bool ContinueTimer { get; set; } = true;

        static bool isRandom;
        public bool IsRandom
        {
            get => isRandom;
            set
            {
                isRandom = value;
                OnPropertyChanged();
                Switch_Toggled_Type();
                if(!pageLoading)
                {
                    var success = int.TryParse(IntervalMinutes, out int result);
                    IntervalIsValid(success);
                    IsPageEnabled = true;
                    OnPropertyChanged("IsPageEnabled");
                }   
            }
        }

        static bool isPageEnabled;
        public bool IsPageEnabled
        {
            get => !IsAlarmEnabled;
            set
            {
                isPageEnabled = value;
                OnPropertyChanged();
            }
        }

        static bool isAlarmEnabled;
        public bool IsAlarmEnabled
        {
            get => isAlarmEnabled;
            set
            {
                isAlarmEnabled = value;
                OnPropertyChanged();
                OnPropertyChanged("AlarmType");
                Switch_Toggled_Enabled();
                if(!pageLoading)
                {
                    SaveAlarmDetails();
                    if (value) 
                    {
                        IsPageEnabled = false;
                        OnPropertyChanged("IsPageEnabled");
                        Opacity = 0.5;
                    }

                    else
                    {
                        IsPageEnabled = true;
                        OnPropertyChanged("IsPageEnabled");
                        Opacity = 1;
                    }

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
            alarmDetails = AlarmRepo.GetItems()
                .SingleOrDefault(x => x.StudyId == Utilities.StudyId) ?? new AlarmDetails();
            intervalTime = alarmDetails.Interval / 60;
            AlarmType = isRandom == false ? interval : AlarmNotificationService.Random;
        }


        void Switch_Toggled_Enabled()
        {
            alarmDetails = AlarmRepo.GetItems()
                .SingleOrDefault(x => x.StudyId == Utilities.StudyId) ?? new AlarmDetails();
            intervalTime = alarmDetails.Interval / 60;
            if (IsAlarmEnabled)
                AlarmStatus = "ENABLED";
            else
                AlarmStatus = "DISABLED";
                
        }

        public AlarmPageViewModel()
        {
            if (!IsPageVisible) return;

            pageLoading = true;
            ActivtySample = SampleRepo.GetItem(Utilities.StudyId);

            StudyType = ActivtySample.IsRated == false ? "RATED" : "UNRATED";

            alarmDetails = AlarmRepo.GetItems()
                .SingleOrDefault(x => x.StudyId == Utilities.StudyId) ?? new AlarmDetails();

            AlarmType = alarmDetails.Type != string.Empty ? alarmDetails.Type : interval;
            IsRandom = alarmDetails.Type != interval;
            IsAlarmEnabled = alarmDetails.IsActive;
            IntervalMinutes = (alarmDetails.Interval / 60).ToString();
            pageLoading = false;
        }

        void SaveAlarmDetails()
        {
            if (IsAlarmEnabled)
            {
                var success = int.TryParse(IntervalMinutes, out int result);

                if(!IntervalIsValid(success)) return;

                intervalTime = result * 60;
            }
            else
                intervalTime = intervalTime * 60;
                
            AlarmNotificationService.SaveNewAlarmDetails(intervalTime, AlarmType, IsAlarmEnabled);

            AlarmNotificationService.AlarmSetFromAlarmPage = true;
        }

        private bool IntervalIsValid(bool success)
        {
            if (!IsAlarmEnabled) return true;

            if (!success)
            {
                ValidationText = "Please enter valid minutes less than 99";
                Opacity = 0.2;
                IsInvalid = true;
                IsAlarmEnabled = false;
                ShowClose = true;
                Switch_Toggled_Enabled();
                return false;

            }
            else
                return true;

        }
    }
}

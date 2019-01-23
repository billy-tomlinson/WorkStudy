using System;
using System.Linq;
using Xamarin.Forms;

namespace WorkStudy.Services
{
    public static class AlarmNotificationService
    {
        public static string Random = "RANDOM";

        static bool restartAlarmCounter;
        public static bool RestartAlarmCounter
        {
            get => restartAlarmCounter;
            set
            {
                restartAlarmCounter = value;
                UpdateAlarm();
            }
        }

        public static int GenerateRandomInterval(int interval)
        {
            Random r = new Random();
            var nextInterval = r.Next(0, interval * 2);
            nextInterval = nextInterval < 60 ? 61 : nextInterval;
            return nextInterval;
        }

        public static void SetNextRandomAlarmTime(int nextAlarm)
        {
            var service = DependencyService.Get<ILocalNotificationService>();

            service.DisableLocalNotification("Alert", "Next Observation Round", 0, DateTime.Now);
            service.LocalNotification("Alert", "Next Observation Round", 0, DateTime.Now, nextAlarm);
        }


        public static void SetNextRandomAlarmTimeAndroid(int nextAlarm)
        {
            var service = DependencyService.Get<ILocalNotificationService>();

            service.LocalNotification("Alert", "Next Observation Round", 0, DateTime.Now, nextAlarm);
        }

        public static void DisableAlarm()
        {
            var service = DependencyService.Get<ILocalNotificationService>();

            service.DisableLocalNotification("Alert", "Next Observation Round", 0, DateTime.Now);
        }

        public static DateTime SaveNewAlarmDetails(int intervalTime, string alarmType, bool isAlarmEnabled)
        {
            var alarmDetails = Utilities.AlarmRepo.GetItems().SingleOrDefault(x => x.StudyId == Utilities.StudyId);

            alarmDetails.Interval = intervalTime;
            alarmDetails.Type = alarmType;
            alarmDetails.IsActive = isAlarmEnabled;
            alarmDetails.StudyId = Utilities.StudyId;

            var nextObsTime = alarmType == Random ? GenerateRandomInterval(intervalTime) : intervalTime;
            alarmDetails.NextNotificationTime = DateTime.Now.AddSeconds(nextObsTime);

            if (isAlarmEnabled)
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        SetNextRandomAlarmTime(nextObsTime);
                        break;
                    case Device.Android:
                        //do nothing
                        SetNextRandomAlarmTimeAndroid(nextObsTime);
                        break;
                }
            if (!isAlarmEnabled)
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        DisableAlarm();
                        break;
                    case Device.Android:
                        //do nothing
                        DisableAlarm();
                        break;
                }

            Utilities.AlarmRepo.SaveItem(alarmDetails);

            return alarmDetails.NextNotificationTime;
        }

        private static void UpdateAlarm()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                if (restartAlarmCounter && Utilities.StudyId > 0)
                {
                    var alarm = Utilities.AlarmRepo.GetItems().SingleOrDefault(x => x.StudyId == Utilities.StudyId);
                    SaveNewAlarmDetails(alarm.Interval, alarm.Type, alarm.IsActive);
                }
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                //do nothing for now
            }
        }

        public static DateTime CheckIfAlarmHasExpiredWhilstInBackgroundOrAlarmOff()
        {
            return UpdateAlarmAfterBeingInBackroundOrAlarmOff();
        }

        public static DateTime AndroidCheckIfAlarmHasExpiredWhilstInBackgroundOrAlarmOff()
        {
            return AndroidUpdateAlarmAfterBeingInBackroundOrAlarmOff();
        }
        private static DateTime UpdateAlarmAfterBeingInBackroundOrAlarmOff()
        {

            DateTime newObsTime = new DateTime();
            if (Utilities.StudyId > 0)
            {
                var alarm = Utilities.AlarmRepo.GetItems().SingleOrDefault(x => x.StudyId == Utilities.StudyId);
                bool notificationExpired = alarm.NextNotificationTime < DateTime.Now;

                if (Device.RuntimePlatform == Device.iOS)
                {
                    if (notificationExpired)
                        newObsTime = SaveNewAlarmDetails(alarm.Interval, alarm.Type, alarm.IsActive);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    //do nothing for now
                    newObsTime = DateTime.Now.AddSeconds(alarm.Interval);
                }
            }

            return newObsTime;
        }

        private static DateTime AndroidUpdateAlarmAfterBeingInBackroundOrAlarmOff()
        {

            DateTime newObsTime = new DateTime();
            if (Utilities.StudyId > 0)
            {
                var alarm = Utilities.AlarmRepo.GetItems().SingleOrDefault(x => x.StudyId == Utilities.StudyId);
                bool notificationExpired = alarm.NextNotificationTime < DateTime.Now;

                if (Device.RuntimePlatform == Device.Android)
                {
                    if (notificationExpired)
                        newObsTime = SaveNewAlarmDetails(alarm.Interval, alarm.Type, alarm.IsActive);
                }    
            }

            return newObsTime;
        }
    }
}

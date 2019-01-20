using System;
using System.Linq;
using Xamarin.Forms;

namespace WorkStudy.Services
{
    public static class AlarmNotificationService
    {
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


        public static void DisableAlarm()
        {
            var service = DependencyService.Get<ILocalNotificationService>();

            service.DisableLocalNotification("Alert", "Next Observation Round", 0, DateTime.Now);
        }

        public static void SaveNewAlarmDetails(int intervalTime, string alarmType, bool isAlarmEnabled)
        {
            var alarmDetails = Utilities.AlarmRepo.GetItems().SingleOrDefault(x => x.StudyId == Utilities.StudyId);

            alarmDetails.Interval = intervalTime;
            alarmDetails.Type = alarmType;
            alarmDetails.IsActive = isAlarmEnabled;
            alarmDetails.StudyId = Utilities.StudyId;

            var nextObsTime = alarmType == "RANDOM" ? GenerateRandomInterval(intervalTime) : intervalTime;
            alarmDetails.NextNotificationTime = DateTime.Now.AddSeconds(nextObsTime);

            Utilities.AlarmRepo.SaveItem(alarmDetails);

            if (isAlarmEnabled)
                SetNextRandomAlarmTime(nextObsTime);
            else
                DisableAlarm();

            Utilities.AlarmRepo.SaveItem(alarmDetails);
        }
    }
}

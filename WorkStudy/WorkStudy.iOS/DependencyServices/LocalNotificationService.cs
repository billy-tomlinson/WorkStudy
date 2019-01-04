using System;
using UIKit;
using Foundation;
using System.Linq;
using WorkStudy.Services;
using WorkStudy.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalNotificationService))]
namespace WorkStudy.iOS.DependencyServices
{
    public class LocalNotificationService : ILocalNotificationService
    {
        const string NotificationKey = "LocalNotificationKey";

        public void LocalNotification(string title, string body, int id, DateTime notifyTime)
        {

            var notification = new UILocalNotification
            {
                AlertTitle = title,
                AlertBody = body,
                SoundName = UILocalNotification.DefaultSoundName,
                FireDate = notifyTime.ToNSDate(),
                //RepeatInterval = NSCalendarUnit.Hour,
                UserInfo = NSDictionary.FromObjectAndKey(NSObject.FromObject(id), NSObject.FromObject(NotificationKey))
            };

            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }

        public void Cancel(int id)
        {
            DisableAlarm(id);
        }

        public void Disable()
        {
            DisableAlarm(0);
        }

        public void DisableLocalNotification(string title, string body, int id, DateTime notifyTime)
        {
            DisableAlarm(id);
        }

        private static void DisableAlarm(int id)
        {
            var notifications = UIApplication.SharedApplication.ScheduledLocalNotifications;
            var notification = notifications.Where(n => n.UserInfo.ContainsKey(NSObject.FromObject(NotificationKey)))
                .FirstOrDefault(n => n.UserInfo[NotificationKey].Equals(NSObject.FromObject(id)));
            UIApplication.SharedApplication.CancelAllLocalNotifications();
            if (notification != null)
            {
                UIApplication.SharedApplication.CancelLocalNotification(notification);
                UIApplication.SharedApplication.CancelAllLocalNotifications();
            }
        }
    }

    public static class DateTimeExtensions
    {

        static DateTime nsUtcRef = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        // last zero is milliseconds

        public static double SecondsSinceNSRefenceDate(this DateTime dt)
        {
            return (dt.ToUniversalTime() - nsUtcRef).TotalSeconds;
        }

        public static NSDate ToNSDate(this DateTime dt)
        {
            return NSDate.FromTimeIntervalSinceReferenceDate(dt.SecondsSinceNSRefenceDate());
        }
    }
}

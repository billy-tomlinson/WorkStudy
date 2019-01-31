using System;
using System.IO;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using WorkStudy.Droid.DependencyServices;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;
using Android.Runtime;

[assembly: Dependency(typeof(LocalNotificationService))]
namespace WorkStudy.Droid.DependencyServices
{
    public class LocalNotificationService : ILocalNotificationService
    {
        private int _notificationIconId { get; set; }
        readonly DateTime jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public void LocalNotification(string title, string body, int id, DateTime notifyTime, double repeatInterval)
        {

            Intent alarmIntent = new Intent(Android.App.Application.Context, typeof(AlarmReceiver));
            alarmIntent.AddFlags(ActivityFlags.SingleTop);
            PendingIntent pending = PendingIntent.GetBroadcast(Android.App.Application.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = Android.App.Application.Context.GetSystemService(Context.AlarmService).JavaCast<AlarmManager>();

            //AlarmType.RtcWakeup – it will fire up the pending intent at a specified time, waking up the device
            alarmManager.SetRepeating(AlarmType.RtcWakeup, BootReceiver.FirstReminder(), BootReceiver.reminderInterval, pending);
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, 0, alarmIntent, 0);
        }

       
        private string SerializeNotification(LocalNotification notification)
        {

            var xmlSerializer = new XmlSerializer(notification.GetType());

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, notification);
                return stringWriter.ToString();
            }
        }

        public void DisableLocalNotification(string title, string body, int id, DateTime notifyTime)
        {
            //var pendingIntent = GeneratePendingIntent(title, body, id, notifyTime);
            //var alarmManager = GetAlarmManager();
            //alarmManager.Cancel(pendingIntent);

            //var notificationManager = NotificationManagerCompat.From(Android.App.Application.Context);
            //notificationManager.CancelAll();
            //notificationManager.Cancel(id);
        }
    }
}

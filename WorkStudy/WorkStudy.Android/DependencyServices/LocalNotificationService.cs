using System;
using System.IO;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Java.Lang;
using WorkStudy.Droid.DependencyServices;
using WorkStudy.Model;
using WorkStudy.Services;
using Plugin.Toasts;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalNotificationService))]
namespace WorkStudy.Droid.DependencyServices
{
    public class LocalNotificationService : ILocalNotificationService
    {
        private int _notificationIconId { get; set; }
        readonly DateTime jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public void LocalNotification(string title, string body, int id, DateTime notifyTime, double repeatInterval)
        {
            long repeatIntervalTime = (long)repeatInterval * 1000;
            var totalMilliSeconds = (long)(notifyTime.ToUniversalTime() - jan1St1970).TotalMilliseconds;
            if (totalMilliSeconds < JavaSystem.CurrentTimeMillis())
            {
                totalMilliSeconds = totalMilliSeconds + repeatIntervalTime;
            }

            var pendingIntent = GeneratePendingIntent(title, body, id, notifyTime);
            var alarmManager = GetAlarmManager();
            alarmManager.SetRepeating(AlarmType.RtcWakeup, totalMilliSeconds, repeatIntervalTime, pendingIntent);
        }

        private PendingIntent GeneratePendingIntent(string title, string body, int id, DateTime notifyTime)
        {
            var intent = CreateIntent(id);
            intent.AddFlags(ActivityFlags.SingleTop);
            var localNotification = new LocalNotification
            {
                Title = title,
                Body = body,
                Id = id,
                NotifyTime = notifyTime,
                IconId = _notificationIconId != 0 ? _notificationIconId : Resource.Drawable.alert
            };

            var serializedNotification = SerializeNotification(localNotification);
            intent.PutExtra(ScheduledAlarmHandler.LocalNotificationKey, serializedNotification);

            var pendingIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, 0, intent, PendingIntentFlags.CancelCurrent);
            return pendingIntent;
        }

        public static Intent GetLauncherActivity()
        {
            var packageName = Android.App.Application.Context.PackageName;
            return Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(packageName);
        }

        private Intent CreateIntent(int id)
        {
            return new Intent(Android.App.Application.Context, typeof(ScheduledAlarmHandler)).SetAction("LocalNotifierIntent" + id);
        }

        private AlarmManager GetAlarmManager()
        {
            var alarmManager = Android.App.Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
            return alarmManager;
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
            var pendingIntent = GeneratePendingIntent(title, body, id, notifyTime);
            var alarmManager = GetAlarmManager();
            alarmManager.Cancel(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(Android.App.Application.Context);
            notificationManager.CancelAll();
            notificationManager.Cancel(id);
        }
    }

    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    public class ScheduledAlarmHandler : BroadcastReceiver
    {

        public const string LocalNotificationKey = "LocalNotification";

        public override void OnReceive(Context context, Intent intent)
        {
            var elapsedTime = DateTime.Now.Subtract(Utilities.LastNotification);
            Utilities.LastNotification = DateTime.Now;
            var options = new NotificationOptions()
            {
                Title = "Alert",
                Description = $"{elapsedTime.Minutes} mins have elapsed - Next Observation",
                IsClickable = true,
                WindowsOptions = new WindowsOptions() { LogoUri = "icon.png" },
                ClearFromHistory = true,
                AllowTapInNotificationCenter = false,
                AndroidOptions = new AndroidOptions()
                {
                    HexColor = "#F99D1C",
                    ForceOpenAppOnNotificationTap = true
                }
            };

            var notificator = DependencyService.Get<IToastNotificator>();

            //notificator.Notify(options);
            AlarmNotificationService.RestartAlarmCounter = true;
            notificator.Notify(HandleAction, options);

        }
        #region  - possible random alarm functionlity - -maybe...
        void HandleAction(INotificationResult obj)
        {
           //DisableAlarmEvent();
            EnableAlarmEvent();
        }

        void EnableAlarmEvent()
        {
            DependencyService.Get<ILocalNotificationService>()
            .LocalNotification("Alert", "Next Observation Round", 0, 
                AlarmNotificationService.NextAlarmTime, AlarmNotificationService.NextIntervalTime);
        }

        void DisableAlarmEvent()
        {
            DependencyService.Get<ILocalNotificationService>()
                .DisableLocalNotification("Alert", "Next Observation Round", 0, AlarmNotificationService.NextAlarmTime);
        }
        #endregion
    }
}

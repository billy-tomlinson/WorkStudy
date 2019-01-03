using System;
using System.IO;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using Java.Lang;
using WorkStudy.Droid.DependencyServices;
using WorkStudy.Model;
using WorkStudy.Services;
using Plugin.Toasts;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(LocalNotificationService))]
namespace WorkStudy.Droid.DependencyServices
{
    public class LocalNotificationService : ILocalNotificationService
    {
        private int _notificationIconId { get; set; }
        readonly DateTime jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public void LocalNotification(string title, string body, int id, DateTime notifyTime)
        {

            var repeatForMinute = 60000;
            var totalMilliSeconds = (long)(notifyTime.ToUniversalTime() - jan1St1970).TotalMilliseconds;
            if (totalMilliSeconds < JavaSystem.CurrentTimeMillis())
            {
                totalMilliSeconds = totalMilliSeconds + repeatForMinute;
            }

            var pendingIntent = GeneratePendingIntent(title, body, id, notifyTime);
            var alarmManager = GetAlarmManager();
            alarmManager.SetRepeating(AlarmType.RtcWakeup, totalMilliSeconds, repeatForMinute, pendingIntent);
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

        public void Cancel(int id)
        {
            var intent = CreateIntent(id);
            var pendingIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, 0 , intent, PendingIntentFlags.CancelCurrent);
            var alarmManager = GetAlarmManager();
            alarmManager.Cancel(pendingIntent);
            var notificationManager = NotificationManagerCompat.From(Android.App.Application.Context);
            notificationManager.CancelAll();
            notificationManager.Cancel(id);
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
            //var notificationId = intent.Extras.GetInt(NotificationBuilder.NotificationId, -1);
            //if (notificationId > -1)
            //{
            //    switch (intent.Action)
            //    {
            //        case NotificationBuilder.OnClickIntent:

            //            try
            //            {
            //                // Attempt to re-focus/open the app.
            //                var doForceOpen = intent.Extras.GetBoolean(NotificationBuilder.NotificationForceOpenApp, false);
            //                if (doForceOpen)
            //                {
            //                    var packageManager = Android.App.Application.Context.PackageManager;
            //                    Intent launchIntent = packageManager.GetLaunchIntentForPackage(NotificationBuilder.PackageName);
            //                    if (launchIntent != null)
            //                    {
            //                        launchIntent.AddCategory(Intent.CategoryLauncher);
            //                        Android.App.Application.Context.StartActivity(launchIntent);
            //                    }
            //                }
            //            }
            //            catch (System.Exception ex)
            //            {
            //                System.Diagnostics.Debug.WriteLine("Failed to re-focus/launch the app: " + ex);
            //            }

            //            // Click
            //            if (NotificationBuilder.EventResult != null && !NotificationBuilder.EventResult.ContainsKey(notificationId.ToString()))
            //            {
            //                NotificationBuilder.EventResult.Add(notificationId.ToString(), new NotificationResult() { Action = NotificationAction.Clicked, Id = notificationId });
            //            }
            //            break;

            //        default:

            //            // Dismiss/Default
            //            if (NotificationBuilder.EventResult != null && !NotificationBuilder.EventResult.ContainsKey(notificationId.ToString()))
            //            {
            //                NotificationBuilder.EventResult.Add(notificationId.ToString(), new NotificationResult() { Action = NotificationAction.Dismissed, Id = notificationId });
            //            }
            //            break;
            //    }

            //    if (NotificationBuilder.ResetEvent != null && NotificationBuilder.ResetEvent.ContainsKey(notificationId.ToString()))
            //    {
            //        NotificationBuilder.ResetEvent[notificationId.ToString()].Set();
            //    }
            //}

            ////var extra = intent.GetStringExtra(LocalNotificationKey);
            ////var notification = DeserializeNotification(extra);
            //////Generating notification
            ////var builder = new NotificationCompat.Builder(Application.Context)
            ////    .SetContentTitle(notification.Title)
            ////    .SetContentText(notification.Body)
            ////    .SetSmallIcon(notification.IconId)
            ////    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone))
            ////    .SetPriority(NotificationCompat.PriorityMax)
            ////    .SetVisibility(1)
            ////    .SetAutoCancel(true);

            ////var resultIntent = LocalNotificationService.GetLauncherActivity();
            ////resultIntent.SetFlags(ActivityFlags.SingleTop);
            ////var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(Application.Context);
            ////stackBuilder.AddNextIntent(resultIntent);

            ////Random random = new Random();
            ////int randomNumber = random.Next(9999 - 1000) + 1000;

            ////var resultPendingIntent =
            ////    stackBuilder.GetPendingIntent(randomNumber, (int)PendingIntentFlags.Immutable);
            ////builder.SetContentIntent(resultPendingIntent);
            ////// Sending notification
            ////var notificationManager = NotificationManagerCompat.From(Application.Context);
            ////notificationManager.Notify(randomNumber, builder.Build());

            var options = new NotificationOptions()
            {
                Title = "The Title Line",
                Description = "The Description Content",
                IsClickable = true,
                WindowsOptions = new WindowsOptions() { LogoUri = "icon.png" },
                ClearFromHistory = false,
                AllowTapInNotificationCenter = false,
                AndroidOptions = new AndroidOptions()
                {
                    HexColor = "#F99D1C",
                    ForceOpenAppOnNotificationTap = true
                }
            };

            var notificator = DependencyService.Get<IToastNotificator>();

            notificator.Notify(options);
           
        }

        private LocalNotification DeserializeNotification(string notificationString)
        {

            var xmlSerializer = new XmlSerializer(typeof(LocalNotification));
            using (var stringReader = new StringReader(notificationString))
            {
                var notification = (LocalNotification)xmlSerializer.Deserialize(stringReader);
                return notification;
            }
        }
    }
}

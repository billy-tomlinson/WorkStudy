using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;

namespace WorkStudy.Droid
{
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {

            NotificationManager mNotificationManager;

            NotificationCompat.Builder mBuilder =
                new NotificationCompat.Builder(context, "notify_001");
            Intent ii = new Intent(context, typeof(AlarmReceiver));
            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, ii, 0);

            NotificationCompat.BigTextStyle bigText = new NotificationCompat.BigTextStyle();
            bigText.BigText("hhhh");
            bigText.SetBigContentTitle("Today's Bible Verse");
            bigText.SetSummaryText("Text in detail");

            mBuilder.SetContentIntent(pendingIntent);
            mBuilder.SetSmallIcon(Resource.Drawable.hourglassempty);
            mBuilder.SetContentTitle("Your Title");
            mBuilder.SetContentText("Your text");
            mBuilder.SetPriority(2);
            mBuilder.SetStyle(bigText);

            mNotificationManager =
                (NotificationManager) context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelId = "YOUR_CHANNEL_ID";
                var channel = new NotificationChannel(channelId,
                    "Channel human readable title",
                        NotificationImportance.Max);
                mNotificationManager.CreateNotificationChannel(channel);
                mBuilder.SetChannelId(channelId);
            }

            mNotificationManager.Notify(0, mBuilder.Build());









            //string CHANNEL_ID = "CHANNEL_ID";
            //var message = "Hello from android";

            //int notifyID = 1;

            //var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);


            //if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            //{
            //    var notificationChannel = new NotificationChannel(CHANNEL_ID, "Work Study", NotificationImportance.Max);
            //    notificationManager.CreateNotificationChannel(notificationChannel);
            //}

            //if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            //{
            //    var importance = NotificationManager.ImportanceHigh;
            //    NotificationChannel mChannel = new NotificationChannel(CHANNEL_ID, name, importance);
            //    notificationManager.createNotificationChannel(mChannel);
            //    Notification.Builder b = new Notification.Builder(BaseActivity.this, CHANNEL_ID);
            //    b.setAutoCancel(true)
            //            .setWhen(System.currentTimeMillis())
            //            .setSmallIcon(icon)
            //            .setContentTitle(title)
            //            .setContentText(content)
            //            .setChannelId(CHANNEL_ID)
            //            .setContentIntent(contentIntent);
            //    Notification notification = b.build();
            //    notificationManager.notify(notificationType, notification);
            //}
            //var intent = new Intent(context, typeof(MainActivity));
            //intent.AddFlags(ActivityFlags.SingleTop);
            //var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            //var bigstyle = new Notification.BigTextStyle();
            //bigstyle.BigText(message);
            //bigstyle.SetBigContentTitle("Work Study");
            //var notificationBuilder = new Notification.Builder(context)
            //.SetSmallIcon(Resource.Drawable.alert) //Icon  
            //.SetContentTitle("Work Study") //Title  
            //.SetContentText(message) //Message  
            //.SetAutoCancel(true)
            //.SetContentIntent(pendingIntent)
            //.SetDefaults(NotificationDefaults.All)
            ////.SetChannelId(CHANNEL_ID)
            //.SetStyle(bigstyle);



            //Notification notification = new Notification.Builder(context)
            //.setContentTitle("New Message")
            //.setContentText("You've received new messages.")
            //.setSmallIcon(R.drawable.ic_notify_status)
            //.setChannelId(CHANNEL_ID)
            //.build();

            //var notification = notificationBuilder.Build();

            //notificationManager.Notify(1331, notification);

            //var title = "Hello world!";
            //var message = "Checkout this notification";

            //Intent backIntent = new Intent(context, typeof(MainActivity));
            //backIntent.SetFlags(ActivityFlags.NewTask);

            ////The activity opened when we click the notification is SecondActivity
            ////Feel free to change it to you own activity
            //var resultIntent = new Intent(context, typeof(SecondActivity));

            //PendingIntent pending = PendingIntent.GetActivities(context, 0,
            //    new Intent[] { backIntent, resultIntent },
            //    PendingIntentFlags.OneShot);

            //var builder =
            //    new Notification.Builder(context)
            //        .SetContentTitle(title)
            //        .SetContentText(message)
            //        .SetAutoCancel(true)
            //        .SetSmallIcon(Resource.Drawable.thumb)
            //        .SetDefaults(NotificationDefaults.All);

            //builder.SetContentIntent(pending);
            //var notification = builder.Build();
            //var manager = NotificationManager.FromContext(context);
            //manager.Notify(1331, notification);

        }
}
}


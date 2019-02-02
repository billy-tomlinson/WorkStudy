﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using WorkStudy.Services;

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
            bigText.BigText("Work Study");
            bigText.SetBigContentTitle("Time for Next Observation.");
            bigText.SetSummaryText("Time for Next Observation.");

            mBuilder.SetContentIntent(pendingIntent);
            mBuilder.SetSmallIcon(Resource.Drawable.hourglassempty);
            mBuilder.SetContentTitle("Work Study");
            mBuilder.SetContentText("Time for Next Observation.");
            mBuilder.SetPriority(2);
            mBuilder.SetStyle(bigText);

            mNotificationManager =
                (NotificationManager)context.GetSystemService(Context.NotificationService);

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

            AlarmNotificationService.RestartAlarmCounter = true;

        }
    }
}


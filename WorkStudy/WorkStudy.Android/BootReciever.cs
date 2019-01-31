using System;

using Android.App;
using Android.Content;

namespace WorkStudy.Droid
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootReceiver : BroadcastReceiver
    {

        //the interval currently every one minute
        //to set it to dayly change the value to 24 * 60 * 60 * 1000
        public static long reminderInterval = 60 * 1000;
        public static long FirstReminder()
        {
            Java.Util.Calendar calendar = Java.Util.Calendar.Instance;
            //calendar.Set(Java.Util.CalendarField.HourOfDay, 18);
            //calendar.Set(Java.Util.CalendarField.Minute, 24);
            //calendar.Set(Java.Util.CalendarField.Second, 00);
            return calendar.TimeInMillis;
        }
        public override void OnReceive(Context context, Intent intent)
        {
            var alarmIntent = new Intent(context, typeof(AlarmReceiver));
            alarmIntent.AddFlags(ActivityFlags.SingleTop);
            var pending = PendingIntent.GetBroadcast(context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

            alarmManager.SetRepeating(AlarmType.RtcWakeup, FirstReminder(), reminderInterval, pending);
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, 0, alarmIntent, 0);
        }
    }
}

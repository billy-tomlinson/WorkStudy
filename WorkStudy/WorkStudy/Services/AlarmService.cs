using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorkStudy.Services
{
    public class AlarmService
    {
    
        public static bool CancelAlarm { get; set; }

        public static bool ContinueTimer { get; set; } = true;

        public static void TurnOffAlarm()
        {
            Vibration.Cancel();
            CancelAlarm = true;
        }

        public static void StartVibrateTimer(string intervalMinutes)
        {
            Random random = new Random();
            var interval = int.Parse(intervalMinutes);
            if (interval <= 0)
                interval = random.Next(3, 10);
            Device.StartTimer(TimeSpan.FromMinutes(interval), SetUpTimer());
        }

        public static Func<bool> SetUpTimer()
        {
            return () =>
            {
                if (!AlarmService.ContinueTimer)
                {
                    return false;
                }
                CancelAlarm = false;
                StartTimer().GetAwaiter();
                return ContinueTimer;
            };
        }

        public static async Task StartTimer()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (AlarmService.CancelAlarm)
                    {
                        Vibration.Cancel();
                        break;
                    }
                    else
                    {
                        Vibration.Vibrate();
                        AlarmService.CancelAlarm = false;
                        await Task.Delay(1000);
                    }
                }
            });
        }

    }
}

using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class VibratePage : ContentPage
    {
        bool cancelAlarm;
        bool continueTimer = true;

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Vibration.Cancel();
            vibrateButton.IsVisible = false;
            cancelAlarm = true;
        }

        void Disable_Clicked(object sender, System.EventArgs e)
        {
            continueTimer = false;
            cancelAlarm = true;
        }
        public VibratePage()
        {
            
            InitializeComponent();
            vibrateButton.IsVisible = false;
            
            Device.StartTimer(TimeSpan.FromSeconds(20), () =>
            {
                cancelAlarm = false;
                vibrateButton.IsVisible = true;
                StartTimer().GetAwaiter();
                return continueTimer; // True = Repeat again, False = Stop the timer
            });
           
        }

        public async Task StartTimer()
        {              
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (cancelAlarm)
                    {
                        Vibration.Cancel();
                        break;
                    }
                    else
                    {
                        Vibration.Vibrate();
                        cancelAlarm = false;
                        await Task.Delay(2000);
                    }  
                }  
            });
        }
    }
}


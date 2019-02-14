using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class StopWatch : ContentPage
    {
        public StopWatch()
        {
            InitializeComponent();
        }

        private bool _isRunning;
        List<string> lapTimes = new List<string>();
        public double dec { get; set; }

        private void btn_Start_Clicked(object sender, System.EventArgs e)
        {
            _isRunning = true;
            RunTimer();
        }

        private void btn_Stop_Clicked(object sender, System.EventArgs e)
        {
            _isRunning = false;
            RunTimer();
        }


        private void btn_ShowLapTimes_Clicked(object sender, System.EventArgs e)
        {

            var scroll = new ScrollView();
            Content = scroll;
            var stack = new StackLayout();
            StackLayout sl = new StackLayout();

            foreach (var item in lapTimes)
            {
                Label label = new Label
                {
                    Text = item
                };
                sl.Children.Add(label);
               
            }

            Content = sl;
        }

        private void btn_Pause_Clicked(object sender, System.EventArgs e)
        {
            try
            {

                string sub1 = dec.ToString().Substring(0, 5);
                string sub = dec.ToString().Substring(4, 1);

                switch (sub)
                {
                   
                    case "0":
                        if (lapTimes.Find(x => x.EndsWith("0", StringComparison.InvariantCultureIgnoreCase)) == null)
                            lapTimes.Add(sub1);
                        break;
                    case "1":
                        if (lapTimes.Find(x => x.EndsWith("1", StringComparison.InvariantCultureIgnoreCase)) == null)
                            lapTimes.Add(sub1);
                        break;
                    case "2":
                        if (lapTimes.Find(x => x.EndsWith("2", StringComparison.InvariantCultureIgnoreCase)) == null)
                            lapTimes.Add(sub1);
                        break;
                    case "3":
                        if (lapTimes.Find(x => x.EndsWith("3", StringComparison.InvariantCultureIgnoreCase)) == null)
                            lapTimes.Add(sub1);
                        break;
                    case "4":
                        if (lapTimes.Find(x => x.EndsWith("4", StringComparison.InvariantCultureIgnoreCase)) == null)
                            lapTimes.Add(sub1);
                        break;
                    case "5":
                        if (lapTimes.Find(x => x.EndsWith("5", StringComparison.InvariantCultureIgnoreCase)) == null)
                            lapTimes.Add(sub1);
                        break;
                    case "6":
                        if (lapTimes.Find(x => x.EndsWith("6", StringComparison.InvariantCultureIgnoreCase)) == null)
                            lapTimes.Add(sub1);
                        break;
                    case "7":
                        if (lapTimes.Find(x => x.EndsWith("7", StringComparison.InvariantCultureIgnoreCase)) == null)
                            lapTimes.Add(sub1);
                        break;
                    case "8":
                        if (lapTimes.Find(x => x.EndsWith("8", StringComparison.InvariantCultureIgnoreCase)) == null)
                            lapTimes.Add(sub1);
                        break;
                    case "9":
                        if (lapTimes.Find(x => x.EndsWith("9", StringComparison.InvariantCultureIgnoreCase)) == null)
                            lapTimes.Add(sub1);
                        break;

                }
            }
            catch (Exception ex)
            {

            }
        }

        public void RunTimer()
        {

            TimeSpan TotalTime;
            TimeSpan TimeElement = new TimeSpan();
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 1), () =>
            {
            if (!_isRunning) return false;

                TotalTime = TotalTime + TimeElement.Add(new TimeSpan(0, 0, 0, 1));
                double ticks = TotalTime.Ticks / 1000000000;
                dec = ticks / 600;

                //double adjustment = Math.Pow(dec, 3);
                //var x = Math.Floor(dec * adjustment) / adjustment;
                //var x = Math.Floor(dec * 100) / 100;
                //dec = decimal.Floor(dec1);
                //dec = decimal.Round(ticks / 60, 5, MidpointRounding.AwayFromZero);

                //StopWatchLabel.Text = dec.ToString("##.###");

                try
                {

                    double ss;

                    Random r = new Random();
                    int rInt = r.Next(0, 9);
                    if (rInt > 0)
                    {
                        ss = (double)rInt / 10000;
                        dec = dec + ss;
                    }

                    string sub1 = dec.ToString().Substring(0, 5);
                    string sub = dec.ToString().Substring(5, 1);

                    //switch (sub)
                    //{
                    //    case "1":
                    //        lapTimes.Add(sub1);
                    //        break;
                    //    case "2":
                    //        lapTimes.Add(sub1);
                    //        break;
                    //    case "3":
                    //        lapTimes.Add(sub1);
                    //        break;
                    //    case "4":
                    //        lapTimes.Add(sub1);
                    //        break;
                    //    case "5":
                    //        lapTimes.Add(sub1);
                    //        break;
                    //    case "6":
                    //        lapTimes.Add(sub1);
                    //        break;
                    //    case "7":
                    //        lapTimes.Add(sub1);
                    //        break;
                    //    case "8":
                    //        lapTimes.Add(sub1);
                    //        break;

                    //}
                }
                catch (Exception ex)
                {

                }
               
                return _isRunning;
            });
        }
    }
}

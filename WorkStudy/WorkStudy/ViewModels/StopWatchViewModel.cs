using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkStudy.Model;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class StopWatchViewModel : BaseViewModel
    {

        public Command StartTimer { get; set; }
        public Command LapTimer { get; set; }
        public Command StopTimer { get; set; }
        public Command ClearLaps { get; set; }

        private bool _isRunning;
        public double dec { get; set; }
        static int Counter;

        public StopWatchViewModel()
        {

            StartTimer = new Command(StartTimerEvent);
            StopTimer = new Command(StopTimerEvent);
            LapTimer = new Command(LapTimerEvent);
            ClearLaps = new Command(ClearLapsEvent);

            LapTimes = new ObservableCollection<LapTime>();
        }

        public void StartTimerEvent()
        {
            if(_isRunning) return;

            Counter = 0;
            _isRunning = true;
            RunTimer();
        }

        public void StopTimerEvent()
        {
            _isRunning = false;
        }

        public void ClearLapsEvent()
        {
            LapTimes = new ObservableCollection<LapTime>();
            OnPropertyChanged("LapTimes");
            Counter = 0;
        }

        public void LapTimerEvent()
        {
            try
            {
                Counter = Counter + 1;
                string sub1 = dec.ToString().Substring(0, 5);
                string sub = dec.ToString().Substring(4, 1);

                lapTimesList.Add(new LapTime { Time = sub1, Count = Counter});
                OnPropertyChanged("LapTimes");
            }
            catch (Exception ex)
            {

            }
        }

        static string stopWatchTime;
        public string StopWatchTime
        {
            get => stopWatchTime;
            set
            {
                stopWatchTime = value;
                OnPropertyChanged();
            }
        }


        static List<LapTime> lapTimesList = new List<LapTime>();

        static ObservableCollection<LapTime> lapTimes;
        public ObservableCollection<LapTime> LapTimes
        {
            get
            {
                return new ObservableCollection<LapTime>(lapTimesList.OrderByDescending(x => x.Count));
            }
            set
            {
                lapTimes = value;
                OnPropertyChanged();
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

                StopWatchTime = dec.ToString("##.###");

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
                }
                catch (Exception ex)
                {

                }

                return _isRunning;
            });
        }
    }
}

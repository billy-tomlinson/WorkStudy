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
        public double currentRunningTime { get; set; }
        public double previousRunningTime { get; set; }
        public double lapTime { get; set; }

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

            LapTimes = new ObservableCollection<LapTime>();
            lapTimesList = new List<LapTime>();

            Counter = 0;
            previousRunningTime = 0;
            currentRunningTime = 0;

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

            lapTimesList = new List<LapTime>();

            Counter = 0;
        }

        public void LapTimerEvent()
        {
            try
            {
                Counter = Counter + 1;
                lapTime = currentRunningTime - previousRunningTime;
                previousRunningTime = currentRunningTime;

                string currentRunningTimeFormatted = currentRunningTime.ToString().Substring(0, 5);
                string lapTimeTimeFormatted = lapTime.ToString().Substring(0, 5);

                lapTimesList.Add(new LapTime { TotalElapsedTime = currentRunningTimeFormatted, Count = Counter, IndividualLapTime = lapTimeTimeFormatted });
                OnPropertyChanged("LapTimes");
            }
            catch (Exception ex)
            {
                lapTimesList.Add(new LapTime { TotalElapsedTime = ex.Message, Count = 0, IndividualLapTime = ex.Message });
                OnPropertyChanged("LapTimes");
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
                currentRunningTime = ticks / 600;

                StopWatchTime = currentRunningTime.ToString("##.###");

                try
                {
                    double ss;

                    Random r = new Random();
                    int rInt = r.Next(0, 9);
                    if (rInt > 0)
                    {
                        ss = (double)rInt / 10000;
                        currentRunningTime = currentRunningTime + ss;
                    }
                }
                catch (Exception ex)
                {

                }

                return _isRunning;
            });
        }
    }
}

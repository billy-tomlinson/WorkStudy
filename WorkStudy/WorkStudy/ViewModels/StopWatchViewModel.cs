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

        private bool IsRunning;
        public double currentRunningTime { get; set; }
        public double previousRunningTime { get; set; }
        public static double pausedRunningTime;
        public double lapTime { get; set; }
        public double CurrentTicks { get; set; }

        static int Counter;

        public StopWatchViewModel()
        {

            StartTimer = new Command(StartTimerEvent);
            StopTimer = new Command(StopTimerEvent);
            LapTimer = new Command(LapTimerEvent);
            ClearLaps = new Command(ClearLapsEvent);
            Override = new Command(OverrideEvent);

            LapTimes = new ObservableCollection<LapTime>();
            lapTimesList = new List<LapTime>();

            Counter = 0;
            previousRunningTime = 0;
            currentRunningTime = 0;
            pausedRunningTime = 0;
            lapTime = 0;
            CurrentTicks = 0;
            StopWatchTime = "0.000";
            IsRunning = false;
            IsStartEnabled = true;
            IsLapEnabled = false;
            IsStopEnabled = false;
            IsClearEnabled = false;
        }

        public void StartTimerEvent()
        {
            IsRunning = true;
            IsStartEnabled = false;
            IsLapEnabled = true;
            IsStopEnabled = true;
            IsClearEnabled = false;
            currentRunningTime = 0;

            RunTimer();
        }

        public void StopTimerEvent()
        {
            IsRunning = false;
            IsStartEnabled = true;
            IsLapEnabled = false;
            IsStopEnabled = false;
            IsClearEnabled = true;
            IsStartEnabled = true;
            pausedRunningTime = currentRunningTime;
        }

        public void ClearLapsEvent()
        {
            ValidationText = "Are you sure you want to clear and reset the stop watch?";
            ShowOkCancel = true;
            IsOverrideVisible = false;
            ShowClose = false;
            Opacity = 0.2;
            CloseColumnSpan = 1;
            IsInvalid = true;
        }

        void OverrideEvent(object sender)
        {
            LapTimes = new ObservableCollection<LapTime>();
            OnPropertyChanged("LapTimes");

            lapTimesList = new List<LapTime>();
            StopWatchTime = "0.000";
            Counter = 0;
            pausedRunningTime = 0;
            previousRunningTime = 0;
            IsLapEnabled = false;
            IsStopEnabled = false;
            IsClearEnabled = false;
            IsStartEnabled = true;


            IsInvalid = false;
            Opacity = 1;
        }

        public void LapTimerEvent()
        {
            try
            {

                IsStartEnabled = false;
                IsLapEnabled = true;
                IsStopEnabled = true;
                IsClearEnabled = false;

                Counter = Counter + 1;
                lapTime = currentRunningTime - previousRunningTime;
                previousRunningTime = currentRunningTime;

                string currentRunningTimeFormatted = currentRunningTime.ToString("0.000");
                string lapTimeTimeFormatted = lapTime.ToString("0.000");

                lapTimesList.Add(new LapTime { TotalElapsedTime = currentRunningTimeFormatted, Count = Counter, IndividualLapTime = lapTimeTimeFormatted });
                OnPropertyChanged("LapTimes");
            }
            catch (Exception ex)
            {
                lapTimesList.Add(new LapTime { TotalElapsedTime = ex.Message, Count = 0, IndividualLapTime = ex.Message });
                OnPropertyChanged("LapTimes");
            }
        }

        static string stopWatchTime = "0.000";
        public string StopWatchTime
        {
            get => stopWatchTime;
            set
            {
                stopWatchTime = value;
                OnPropertyChanged();
            }
        }

        static bool isStartEnabled;
        public bool IsStartEnabled
        {
            get => isStartEnabled;
            set
            {
                isStartEnabled = value;
                OnPropertyChanged();
            }
        }

        static bool isStopEnabled;
        public bool IsStopEnabled
        {
            get => isStopEnabled;
            set
            {
                isStopEnabled = value;
                OnPropertyChanged();
            }
        }

        static bool isLapEnabled;
        public bool IsLapEnabled
        {
            get => isLapEnabled;
            set
            {
                isLapEnabled = value;
                OnPropertyChanged();
            }
        }

        static bool isClearEnabled;
        public bool IsClearEnabled
        {
            get => isClearEnabled;
            set
            {
                isClearEnabled = value;
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
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 100), () =>
            {
                if (!IsRunning) return false;

                TotalTime = TotalTime + TimeElement.Add(new TimeSpan(0, 0, 0, 1));

                CurrentTicks = TotalTime.Ticks / 10000000;

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        currentRunningTime = pausedRunningTime + CurrentTicks / 600;
                        break;

                    case Device.Android:
                        currentRunningTime = pausedRunningTime + CurrentTicks / 590; //this is a hack/sweetspot as anroid is slightly slower
                        break;
                }

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

                    StopWatchTime = currentRunningTime.ToString("0.000");
                }
                catch (Exception ex)
                {

                }

                return IsRunning;
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        public double currentRunningTimeSync { get; set; }
        public double previousRunningTime { get; set; }
        public double timeWhenLapButtonClicked { get; set; }
        public double androidLapAdjustment { get; set; }
        public static double pausedRunningTime;
        public double lapTime { get; set; }
        public double CurrentTicks { get; set; }
        public TimeSpan StartTime { get; set; }

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
            StartTime = DateTime.Now.TimeOfDay;
            StartTimeFormatted = StartTime.ToString(@"c");
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

            IsStartEnabled = false;
            IsLapEnabled = true;
            IsStopEnabled = true;
            IsClearEnabled = false;

            var timeElaspedSinceStart = DateTime.Now.TimeOfDay - StartTime;

            var ttt = timeElaspedSinceStart.Ticks / 1000000;
            RealTimeTicks = (double)ttt / 600;
            CurrentTimeFormatted = timeElaspedSinceStart.ToString();
            CurrentTimeFormattedDecimal = RealTimeTicks.ToString("0.000");

            Counter = Counter + 1;

            lapTime = RealTimeTicks - timeWhenLapButtonClicked;

            double randomToForceRounding;

            Random r = new Random();
            int rInt = r.Next(0, 9);
            if (rInt > 0)
            {
                randomToForceRounding = (double)rInt / 10000;
                lapTime = lapTime + randomToForceRounding;
            }

            string currentRunningTimeFormatted = currentRunningTime.ToString("0.000");
            string lapTimeTimeFormatted = lapTime.ToString("0.000");
            
            lapTimesList.Add(new LapTime { TotalElapsedTime = CurrentTimeFormattedDecimal, Count = Counter, IndividualLapTime = lapTimeTimeFormatted });
            OnPropertyChanged("LapTimes");

            timeWhenLapButtonClicked = RealTimeTicks;
            previousRunningTime = (double)DateTime.Now.TimeOfDay.Ticks / 1000000000000;

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

        static string startTimeFormatted;
        public string StartTimeFormatted
        {
            get => startTimeFormatted;
            set
            {
                startTimeFormatted = value;
                OnPropertyChanged();
            }
        }
        static string currentTimeFormatted;
        public string CurrentTimeFormatted
        {
            get => currentTimeFormatted;
            set
            {
                currentTimeFormatted = value;
                OnPropertyChanged();
            }
        }

        static string currentTimeFormattedDecimal;
        public string CurrentTimeFormattedDecimal
        {
            get => currentTimeFormattedDecimal;
            set
            {
                currentTimeFormattedDecimal = value;
                OnPropertyChanged();
            }
        }

        static double realTimeTicks;
        public double RealTimeTicks
        {
            get => realTimeTicks;
            set
            {
                realTimeTicks = value;
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

                var timeElaspedSinceStart = DateTime.Now.TimeOfDay - StartTime;

                var realTicks = timeElaspedSinceStart.Ticks / 1000000;

                RealTimeTicks = (double)realTicks / 600;

                StopWatchTime = RealTimeTicks.ToString("0.000");

                currentRunningTime = pausedRunningTime + CurrentTicks / 600;
               
                return IsRunning;
            });
        }
    }
}

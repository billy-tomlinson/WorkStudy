using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StopWatch.Services;
using WorkStudy.Services;
using Xamarin.Forms;

namespace StopWatch
{
    public class StopWatchViewModel : BaseViewModel
    {

        public Command StartTimer { get; set; }
        public Command LapTimer { get; set; }
        public Command StopTimer { get; set; }
        public Command ClearLaps { get; set; }
        public Command SwitchTimeFormat { get; set; }

        private bool IsRunning;
        public double timeWhenLapButtonClicked { get; set; }
        public double timeWhenStopButtonClicked { get; set; }
        public double lapTime { get; set; }
        public double CurrentTicks { get; set; }
        public TimeSpan StartTime { get; set; }
        public const string Imperial = "%h\\:%m\\:ss\\.ff";
        public const string CentiMinute = "#00.000";

        static int Counter;

        public StopWatchViewModel()
        {

            SwitchTimeFormat = new Command(SwitchTimeFormatEvent);
            StartTimer = new Command(StartTimerEvent);
            StopTimer = new Command(StopTimerEvent);
            LapTimer = new Command(LapTimerEvent);
            ClearLaps = new Command(ClearLapsEvent);
            Override = new Command(OverrideEvent);

            LapTimes = new ObservableCollection<LapTime>();
            LapTimesList = new List<LapTime>();

            Counter = 0;
            lapTime = 0;
            CurrentTicks = 0;
            StopWatchTime = "00.000";
            IsRunning = false;
            IsStartEnabled = true;
            IsLapEnabled = false;
            IsStopEnabled = false;
            IsClearEnabled = false;
            IsPageEnabled = true;
            StopWatchColour = Color.White;
            RealTimeTicks = 0;
            TimerService.Stop();
        }

        public void StartTimerEvent()
        {
            IsRunning = true;
            IsStartEnabled = false;
            IsLapEnabled = true;
            IsStopEnabled = true;
            IsClearEnabled = false;
            StartTime = DateTime.Now.TimeOfDay;
            StartTimeFormatted = StartTime.ToString(@"c");
            RunTimer();
        }

        public void SwitchTimeFormatEvent()
        {
            IsImperial = !IsImperial;

            StopWatchColour = IsImperial ? Color.Black : Color.White;
            StopWatchTime = FormattedStopWatchTime();

            var newLapTimes = new List<LapTime>();

            foreach (var item in LapTimesList)
            {
                var newLapTime = new LapTime()
                {
                    IndividualLapDouble = item.IndividualLapDouble,
                    IndividualLapImperial = item.IndividualLapImperial,
                    TotalElapsedTimeDouble = item.TotalElapsedTimeDouble,
                    TotalElapsedTimeImperial = item.TotalElapsedTimeImperial,
                    IndividualLapTime = IsImperial ? item.IndividualLapImperial.ToString(Imperial) : item.IndividualLapDouble.ToString(CentiMinute),
                    TotalElapsedTime = IsImperial ? item.TotalElapsedTimeImperial.ToString(Imperial) : item.TotalElapsedTimeDouble.ToString(CentiMinute),
                    Count = item.Count
                };

                newLapTimes.Add(newLapTime);
            }

            LapTimesList = newLapTimes;

            OnPropertyChanged("LapTimes");

            Utilities.LapButtonClicked = true;
        }

        private string FormattedStopWatchTime()
        {
            return IsImperial ? TimeSpan.FromMinutes(RealTimeTicks).ToString("%h\\:%m\\:ss\\.f") : RealTimeTicks.ToString(CentiMinute);
        }

        public void StopTimerEvent()
        {
            IsRunning = false;
            IsStartEnabled = true;
            IsLapEnabled = false;
            IsStopEnabled = false;
            IsClearEnabled = true;
            IsStartEnabled = true;

            timeWhenStopButtonClicked = RealTimeTicks;
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
            IsPageEnabled = false;
        }

        void OverrideEvent(object sender)
        {
            LapTimes = new ObservableCollection<LapTime>();
            OnPropertyChanged("LapTimes");

            LapTimesList = new List<LapTime>();
            OnPropertyChanged("LapTimes");
            StopWatchTime = IsImperial ? "0:0:00:00" : "00.000";
            Counter = 0;
            IsLapEnabled = false;
            IsStopEnabled = false;
            IsClearEnabled = false;
            IsStartEnabled = true;
            IsPageEnabled = true;

            timeWhenStopButtonClicked = 0;
            timeWhenLapButtonClicked = 0;
            lapTime = 0;

            IsInvalid = false;
            Opacity = 1;
            RealTimeTicks = 0;
            TimerService.Stop();
        }

        public void LapTimerEvent()
        {

            IsStartEnabled = false;
            IsLapEnabled = true;
            IsStopEnabled = true;
            IsClearEnabled = false;

            Random r = new Random();
            int rInt = r.Next(1, 9);

            double randomToForceRounding = (double)rInt / 10000;

            var timeElaspedSinceStart = DateTime.Now.TimeOfDay - StartTime;

            var timeElaspedSinceStartDecimal = timeElaspedSinceStart.Ticks / 1000000;
            RealTimeTicks = timeWhenStopButtonClicked + randomToForceRounding + (double)timeElaspedSinceStartDecimal / 600;
            CurrentTimeFormatted = timeElaspedSinceStart.ToString();
            CurrentTimeFormattedDecimal = RealTimeTicks.ToString(CentiMinute);

            Counter = Counter + 1;

            lapTime = RealTimeTicks - timeWhenLapButtonClicked;

            var individualLapImperial = TimeSpan.FromMinutes(lapTime);
            var totalElapsedTimeImperial = TimeSpan.FromMinutes(RealTimeTicks);

            lapTime = lapTime + randomToForceRounding;

            CalculateElapsedAndLapCombinedMatch();

            var lapObservation = new LapTime
            {
                IndividualLapTime = IsImperial ? individualLapImperial.ToString(Imperial) : lapTime.ToString(CentiMinute),
                TotalElapsedTime = IsImperial ? totalElapsedTimeImperial.ToString(Imperial) : RealTimeTicks.ToString(CentiMinute),
                Count = Counter,
                IndividualLapDouble = lapTime,
                IndividualLapImperial = individualLapImperial,
                TotalElapsedTimeDouble = RealTimeTicks,
                TotalElapsedTimeImperial = totalElapsedTimeImperial
            };

            LapTimesList.Add(lapObservation);

            OnPropertyChanged("LapTimes");

            timeWhenLapButtonClicked = RealTimeTicks;

            Utilities.LapButtonClicked = true;
        }

        private void CalculateElapsedAndLapCombinedMatch()
        {
            decimal lastTimeWhenClicked = (decimal)Math.Round(timeWhenLapButtonClicked, 3);
            decimal currentLapTime = (decimal)Math.Round(lapTime, 3);
            decimal combinedTimes = (decimal)Math.Round(lastTimeWhenClicked + currentLapTime, 3);
            decimal formattedRealTimeTicks = (decimal)Math.Round(RealTimeTicks, 3);

            if (combinedTimes != formattedRealTimeTicks)
            {
                if (combinedTimes < formattedRealTimeTicks)
                {
                    var difference = formattedRealTimeTicks - combinedTimes;
                    lapTime = lapTime - (double)difference;
                }
                else
                {
                    var difference = combinedTimes - formattedRealTimeTicks;
                    lapTime = lapTime - (double)difference;
                }
            }

            var lapTimeCheck = lapTime < 0 ? 0 : lapTime;
            lapTime = lapTimeCheck;
        }

        static string stopWatchTime = "00.000";
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


        static bool isImperial;
        public bool IsImperial
        {
            get => isImperial;
            set
            {
                isImperial = value;
                OnPropertyChanged();
            }
        }


        static Color stopWatchColour;
        public Color StopWatchColour
        {
            get => stopWatchColour;
            set
            {
                stopWatchColour = value;
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
        static List<LapTime> LapTimesList = new List<LapTime>();

        static ObservableCollection<LapTime> lapTimes;
        public ObservableCollection<LapTime> LapTimes
        {
            get
            {
                return new ObservableCollection<LapTime>(LapTimesList.OrderByDescending(x => x.Count));
            }
            set
            {
                lapTimes = value;
                OnPropertyChanged();
            }
        }

        private Func<bool> CallBackForTimer()
        {
            TimeSpan TotalTime;
            TimeSpan TimeElement = new TimeSpan();

            return () =>
            {
                if (!IsRunning) return false;

                TotalTime = TotalTime + TimeElement.Add(new TimeSpan(0, 0, 0, 1));

                var timeElaspedSinceStart = DateTime.Now.TimeOfDay - StartTime;

                var realTicks = timeElaspedSinceStart.Ticks / 1000000;

                RealTimeTicks = timeWhenStopButtonClicked + (double)realTicks / 600;

                StopWatchTime = FormattedStopWatchTime();

                return IsRunning;
            };
        }

        public void RunTimer()
        {
            TimerService.StartTimer(new TimeSpan(0, 0, 0, 0, 100), CallBackForTimer());
        }
    }
}

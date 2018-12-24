using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {      
        public event PropertyChangedEventHandler PropertyChanged;
        public Command CloseView { get; set; }
        public Command Override { get; set; }

        private readonly string conn;

        public Operator Operator;

        public BaseViewModel(string conn = null)
        {
            this.conn = conn;
            SubmitDetails = new Command(SubmitDetailsAndNavigate);
            CloseView = new Command(CloseValidationView);
            EnsureTableCreation();
            InvalidText = "Please create a new study or select an existing one.";
            IsPageVisible = (Utilities.StudyId > 0 && !Utilities.IsCompleted);
        }

        public bool CancelAlarm { get; set; }

        public bool ContinueTimer { get; set; } = true;

        public Command SubmitDetails { get; set; }

        public IBaseRepository<Operator> OperatorRepo => new BaseRepository<Operator>(conn);

        public IBaseRepository<Observation> ObservationRepo => new BaseRepository<Observation>(conn);

        public IBaseRepository<Activity> ActivityRepo  => new BaseRepository<Activity>(conn);

        public IBaseRepository<MergedActivities> MergedActivityRepo => new BaseRepository<MergedActivities>(conn);

        public IBaseRepository<ActivitySampleStudy> SampleRepo => new BaseRepository<ActivitySampleStudy>(conn);

        public IBaseRepository<ObservationRoundStatus> ObservationRoundStatusRepo => new BaseRepository<ObservationRoundStatus>(conn);

        public TimeSpan CurrentTime => DateTime.Now.TimeOfDay;


        static ObservableCollection<Activity> activities;
        public ObservableCollection<Activity> Activities
        {
            get => activities;
            set
            {
                activities = value;
                OnPropertyChanged();
            }
        }

        static ObservableCollection<MultipleActivities> _groupActivities;
        public ObservableCollection<MultipleActivities> GroupActivities
        {
            get => _groupActivities;
            set
            {
                _groupActivities = value;
                OnPropertyChanged();
            }
        }

        bool isInvalid = false;
        public bool IsInvalid
        {
            get { return isInvalid; }
            set
            {
                isInvalid = value;
                OnPropertyChanged();
            }
        }


        double opacity = 1;
        public double Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                OnPropertyChanged();
            }
        }

        string invalidText;
        public string InvalidText
        {
            get { return invalidText; }
            set
            {
                invalidText = value;
                OnPropertyChanged();
            }
        }

        int studyNumber;
        public int StudyNumber
        {
            get { return Utilities.StudyId; }
            set
            {
                studyNumber = value;
                OnPropertyChanged();
            }
        }

        int closeColumnSpan = 2;
        public int CloseColumnSpan
        {
            get { return closeColumnSpan; }
            set
            {
                closeColumnSpan = value;
                OnPropertyChanged();
            }
        }

        int totalObservationsRequired;
        public int TotalObservationsRequired
        {
            get { return totalObservationsRequired; }
            set
            {
                totalObservationsRequired = value;
                OnPropertyChanged();
            }
        }


        int totalObservationsTaken;
        public int TotalObservationsTaken
        {
            get { return totalObservationsTaken; }
            set
            {
                totalObservationsTaken = value;
                OnPropertyChanged();
            }
        }

        string totalOperatorPercentage;
        public string TotalOperatorPercentage
        {
            get { return totalOperatorPercentage; }
            set
            {
                totalOperatorPercentage = value;
                OnPropertyChanged();
            }
        }

        bool isPageVisible = false;
        public bool IsPageVisible
        {
            get { return isPageVisible; }
            set
            {
                isPageVisible = value;
                IsPageUnavailableVisible = !value;
                OnPropertyChanged();
            }
        }

        bool isOverrideVisible = false;
        public bool IsOverrideVisible
        {
            get { return isOverrideVisible; }
            set
            {
                isOverrideVisible = value;
                IsPageUnavailableVisible = !value;
                OnPropertyChanged();
            }
        }

        int isItemEnabled;
        public int IsItemEnabled
        {
            get { return isItemEnabled; }
            set
            {
                isItemEnabled = value;
                OnPropertyChanged();
            }
        }

        bool isPageUnavailableVisible = false;
        public bool IsPageUnavailableVisible
        {
            get { return isPageUnavailableVisible; }
            set
            {
                isPageUnavailableVisible = value;
                OnPropertyChanged();
            }
        }

        private string validationText;
        public string ValidationText
        {
            get => validationText;
            set
            {
                validationText = value;
                OnPropertyChanged();
            }
        }

        static string alarmStatus;
        public string AlarmStatus
        {
            get => alarmStatus;
            set
            {
                alarmStatus = value;
                OnPropertyChanged();
            }
        }

        static string intervalMinutes;
        public string IntervalMinutes
        {
            get => intervalMinutes;
            set
            {
                intervalMinutes = value;
                OnPropertyChanged();
            }
        }

        public bool StudyInProcess
        {
            get => Get_Observations_By_StudyId().Count > 0;
        }
              
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SubmitDetailsAndNavigate(){}

        public ObservableCollection<Activity> Get_Rated_Enabled_Activities()
        {
            return new ObservableCollection<Activity>(ActivityRepo.GetItems()
                                         .Where(x => x.IsEnabled && x.Rated && x.StudyId == Utilities.StudyId));
        }

        public ObservableCollection<Activity> Get_All_Enabled_Activities()
        {
            return new ObservableCollection<Activity>(ActivityRepo.GetItems()
                                         .Where(x => x.IsEnabled && x.StudyId == Utilities.StudyId));
        }

        public List<Observation> Get_Observations_By_StudyId()
        {
            return ObservationRepo.GetItems()
                               .Where(x => x.StudyId == Utilities.StudyId).ToList();
        }

        public ObservableCollection<Activity> Get_Enabled_Activities()
        {
            return new ObservableCollection<Activity>(ActivityRepo.GetItems()
                .Where(x => x.IsEnabled && x.StudyId == Utilities.StudyId));
        }

        public ObservableCollection<Activity> Get_Rated_Enabled_Activities_WithChildren()
        {
            return new ObservableCollection<Activity>(ActivityRepo.GetAllWithChildren()
                                        .Where(x => x.IsEnabled && x.Rated && x.StudyId == Utilities.StudyId));
        }

        public ObservableCollection<Activity> Get_All_Enabled_Activities_WithChildren()
        {
            return new ObservableCollection<Activity>(ActivityRepo.GetAllWithChildren()
                .Where(x => x.IsEnabled && x.StudyId == Utilities.StudyId));
        }

        public ObservableCollection<Activity> ConvertListToObservable(List<Activity> list1)
        {
            return new ObservableCollection<Activity>(list1.OrderBy(x => x.Id).Where(x => x.IsEnabled));
        }

        private void EnsureTableCreation()
        {
            OperatorRepo.CreateTable();
            ObservationRepo.CreateTable();
            ActivityRepo.CreateTable();
            MergedActivityRepo.CreateTable();
            SampleRepo.CreateTable();
            ObservationRoundStatusRepo.CreateTable();
        }
       
        public void CloseValidationView()
        {
            Opacity = 1;
            IsInvalid = false;
        }

        public List<OperatorRunningTotal> GetRunningTotals(Operator op)
        {
            var totals = new List<OperatorRunningTotal>();

            var observations = op.Observations;
            var totalObs = observations.Count;

            var observationsTaken = ObservationRepo.GetItems().Where(x => x.OperatorId == op.Id).ToList();
            TotalObservationsTaken = totalObs;

            var activtyIds = observationsTaken.Select(x => new { Id = x.AliasActivityId })
                                              .Distinct().ToList();

            var distinctActivities = new List<dynamic>();

            foreach (var item in activtyIds)
            {
                distinctActivities.Add(item);
            }

            var totalRequiredForOperator = 0;

            foreach (var item in distinctActivities)
            {
                var count = observations.Count(x => x.AliasActivityId == item.Id);
                double percentage = count > 0 ? (double)count / totalObs : 0;
                percentage = Math.Round(percentage * 100, 1);

                var totalRequired = Utilities.CalculateObservationsRequired(percentage);

                totalRequiredForOperator = totalRequiredForOperator + totalRequired;

                var runningTotal = new OperatorRunningTotal()
                {
                    ActivityId = item.Id,
                    OperatorId = op.Id,
                    ActivityName = ActivityRepo.GetItem(item.Id).Name,
                    NumberOfObservations = count,
                    ObservationsRequired = totalRequired,
                    Percentage = percentage,
                    PercentageFormatted = $"{percentage.ToString(CultureInfo.InvariantCulture)}%"
                };

                totals.Add(runningTotal);
            }

            TotalOperatorPercentage = string.Empty;
            TotalObservationsRequired = totalRequiredForOperator;
            double totalPercentage = 0;

            if(TotalObservationsRequired > 0)
            {
                totalPercentage = Math.Ceiling((double)TotalObservationsTaken / TotalObservationsRequired * 100);
                TotalOperatorPercentage = $"{totalPercentage.ToString(CultureInfo.InvariantCulture)}%";
            }

            return totals;
        }

        public void TurnOffAlarm()
        {
            Vibration.Cancel();
            CancelAlarm = true;
        }

        public void StartVibrateTimer()
        {
            Random random = new Random();
            var interval = int.Parse(IntervalMinutes);
            if (interval <= 0)
                interval = random.Next(3, 10);
            Device.StartTimer(TimeSpan.FromMinutes(interval), SetUpTimer());
        }

        private Func<bool> SetUpTimer()
        {
            return () =>
            {
                if (!ContinueTimer)
                {
                    return false;
                }
                CancelAlarm = false;
                StartTimer().GetAwaiter();
                return ContinueTimer;
            };
        }

        private async Task StartTimer()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (CancelAlarm)
                    {
                        Vibration.Cancel();
                        break;
                    }
                    else
                    {
                        Vibration.Vibrate();
                        CancelAlarm = false;
                        await Task.Delay(1000);
                    }
                }
            });
        }
    }
}

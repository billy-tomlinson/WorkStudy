using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using WorkStudy.Model;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{

    public class MainPageViewModel : BaseViewModel
    {
        private const string FormatMinutes = "HH:mm:ss";
        private const string FormatMinutesWithoutSeconds = "HH:mm";
        private OperatorObservation operator1;
        List<Observation> observations = new List<Observation>();

        public Command SaveObservations { get; set; }
        public Command ActivitySelected { get; set; }
        public Command RatingSelected { get; set; }
        public Command EndStudy { get; set; }
        public Command PauseStudy { get; set; }
        public Command EditStudy { get; set; }
        public Command CloseActivitiesView { get; set; }
        public Command SettingsSelected { get; set; }
        public Command SaveComment { get; set; }
        public Command CancelComment { get; set; }

        public MainPageViewModel(string conn) : base(conn)
        {
            ConstructorSetUp();
        }

        public MainPageViewModel()
        {
            ConstructorSetUp();
        }

        private void ConstructorSetUp()
        {
            SaveObservations = new Command(SaveObservationDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);
            RatingSelected = new Command(RatingSelectedEvent);
            EndStudy = new Command(TerminateStudy);
            EditStudy = new Command(EditStudyDetails);
            PauseStudy = new Command(NavigateToStudyMenu);
            Override = new Command(OverrideEvent);
            CloseActivitiesView = new Command(CloseActivitiesViewEvent);
            SettingsSelected = new Command(AddSelectedEvent);
            SaveComment = new Command(SaveCommentDetails);
            CancelComment = new Command(CancelCommentDetails);

            Opacity = 1.0;

            Operators = GetAllEnabledOperators();

            GetObservationRound();

            Activities = Get_Enabled_Activities();

            IEnumerable<Activity> obsCollection = Activities;

            var list1 = new List<Activity>(obsCollection);

            foreach (var activity in list1)
            {
                activity.Colour = Color.FromHex(activity.ItemColour);
            };

            Activities = ConvertListToObservable(list1);

            GroupActivities = Utilities.BuildGroupOfActivities(Activities);

            IsPageVisible = IsStudyValid();

            CreateOperatorObservations();

            TotalPercent = (int)GetStudyTotalPercent();

            var obsStatus = ObservationRoundStatusRepo.GetItems()
                            .FirstOrDefault(x => x.ObservationId == ObservationRound);

            ObservationRoundStatus = obsStatus == null ? new ObservationRoundStatus() : obsStatus;

            SetUpNextObservationTimeWithTimer();

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CurrentTime = DateTime.Now.ToString("HH.mm:ss");
                });
                return true;
            });

            IsPageEnabled = true;

            IsCancelEnabled = true;

            RunningTotalsVisible = false;

            Utilities.MoveObservationsToHistoryTable();
        }

        private void SetUpNextObservationTimeWithTimer()
        {
            if (Utilities.StudyId > 0)
            {
                AlarmNotificationService.CheckIfAlarmHasExpiredWhilstInBackgroundOrAlarmOff();
                var alarmDetails = AlarmRepo.GetItems()
                    .SingleOrDefault(x => x.StudyId == Utilities.StudyId);

                AlarmStatus = alarmDetails.IsActive ? "ENABLED" : "DISABLED";

                TimeOfNextObservation = alarmDetails.NextNotificationTime.ToString(FormatMinutes);

                Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                {
                    if(Utilities.StudyId > 0)
                    {
                        AlarmNotificationService.CheckIfAlarmHasExpiredWhilstInBackgroundOrAlarmOff();
                        var alarm = AlarmRepo.GetItems().SingleOrDefault(x => x.StudyId == Utilities.StudyId);
                        if(alarm != null)
                        {
                            var time = alarm.NextNotificationTime.ToString(FormatMinutes);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                TimeOfNextObservation = time;
                                AlarmNotificationService.RestartAlarmCounter = false;
                                AlarmNotificationService.AlarmSetFromAlarmPage = false;
                            });
                        }
                    }
                    return true;
                });

            }
        }

        private void SetNextRandomAlarmTime()
        {
            var alarm = AlarmRepo.GetItems().SingleOrDefault(x => x.StudyId == Utilities.StudyId);

            bool notificationExpired = alarm.NextNotificationTime < DateTime.Now;

            if (alarm.IsActive && alarm.Type == AlarmNotificationService.Random && notificationExpired)
            {
                TimeOfNextObservation = 
                    AlarmNotificationService.SaveNewAlarmDetails(alarm.Interval, alarm.Type, alarm.IsActive)
                    .ToString(FormatMinutes);
            }
            if (alarm.IsActive && alarm.Type != AlarmNotificationService.Random && notificationExpired)
                TimeOfNextObservation = DateTime.Now.AddSeconds(alarm.Interval).ToString(FormatMinutes);
        }

        private Observation Observation { get; set; }
        private int ActivityId { get; set; }
        private int Rating { get; set; }


        static int _observationRound = 1;
        public int ObservationRound
        {
            get => _observationRound;
            set
            {
                _observationRound = value;
                OnPropertyChanged();
            }
        }

        static ObservableCollection<Operator> _operators;
        public ObservableCollection<Operator> Operators
        {
            get => _operators;
            set
            {
                _operators = value;
                OnPropertyChanged();
            }
        }

        static ObservableCollection<OperatorObservation> _operatorObservations;
        public ObservableCollection<OperatorObservation> OperatorObservations
        {
            get => _operatorObservations;
            set
            {
                _operatorObservations = value;
                OnPropertyChanged();
            }
        }

        static bool activitiesVisible;
        public bool ActivitiesVisible
        {
            get => activitiesVisible;
            set
            {
                activitiesVisible = value;
                OnPropertyChanged();
            }
        }

        static bool requestToTerminateStudy;
        public bool RequestToTerminateStudy
        {
            get => requestToTerminateStudy;
            set
            {
                requestToTerminateStudy = value;
                OnPropertyChanged();
            }
        }

        static int totalPercent;
        public int TotalPercent
        {
            get => totalPercent;
            set
            {
                totalPercent = value;
                OnPropertyChanged();
                OnPropertyChanged("TotalPercentFormatted");
            }
        }

        static string totalPercentFormatted;
        public string TotalPercentFormatted
        {
            get => $"{TotalPercent}%";
            set
            {
                totalPercentFormatted = value;
                OnPropertyChanged();
            }

        }

        static string operatorName;
        public string OperatorName
        {
            get => operatorName;
            set
            {
                operatorName = value;
                OnPropertyChanged();
            }
        }

        static bool ratingsVisible;
        public bool RatingsVisible
        {
            get => ratingsVisible;
            set
            {
                ratingsVisible = value;
                OnPropertyChanged();
            }
        }

        public bool AllObservationsTaken
        {
            get => !OperatorObservations.Any(cw => cw.ActivityName == null);

        }

        static Activity currentActivity;
        public Activity CurrentActivity
        {
            get => currentActivity;
            set
            {
                currentActivity = value;
                OnPropertyChanged();
            }
        }


        static ObservationRoundStatus observationRoundStatus;
        public ObservationRoundStatus ObservationRoundStatus
        {
            get => observationRoundStatus;
            set
            {
                observationRoundStatus = value;
                OnPropertyChanged();
            }
        }

        public void UpdateObservationRound()
        {
            ObservationRound = ObservationRound + 1;
        }

        public void SaveObservationDetails()
        {
            if (AllObservationsTaken)
            {
                UpdateObservationRoundStatus();
                SetUpForNextObservationRound();
            }
            else
            {
                Utilities.AllObservationsTaken = false;
                ValidationText = "Not All Operators have been observed.";
                IsOverrideVisible = true;
                ShowClose = true;
                ShowOkCancel = false;
                Opacity = 0.2;
                IsPageUnavailableVisible = false;
                IsInvalid = true;
                CloseColumnSpan = 1;
                RequestToTerminateStudy = false;
                IsPageEnabled = false;
            }
        }

        private void UpdateObservationRoundStatus()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    SetNextRandomAlarmTime();
                    break;
                case Device.Android:
                    //do nothing for now
                    break;
            }

            ObservationRoundStatus.Status = "Complete";
            ObservationRoundStatusRepo.SaveItem(ObservationRoundStatus);
        }

        private void RemoveObservationForOperator()
        {
            RemoveObservation(OperatorName, ObservationRound);

            Operators = GetAllEnabledOperators();

            CreateOperatorObservations();

            Utilities.MainPageHasUpdatedObservationChanges = true;
        }

        private void SetUpForNextObservationRound()
        {
            Utilities.SetUpForNextObservationRound = true;
            Utilities.AllObservationsTaken = true;
            observations = new List<Observation>();
            UpdateObservationRound();
            CreateOperatorObservations();
            TotalPercent = (int)GetStudyTotalPercent();
        }

        void TerminateStudy()
        {
            if (GetStudyTotalPercent() < 100)
            {
                ValidationText = "Study has not reached Limits Of Accuracy. Override?";
                IsOverrideVisible = true;
                ShowClose = true;
                ShowOkCancel = false;
                IsPageUnavailableVisible = false;
                Opacity = 0.2;
                CloseColumnSpan = 1;
                IsInvalid = true;
                RequestToTerminateStudy = true;
                IsPageEnabled = false;
                return;
            }

            if (GetStudyTotalPercent() >= 100)
            {
                ValidationText = "Are you sure you want to finish the study?";
                ShowOkCancel = true;
                IsOverrideVisible = false;
                ShowClose = false;
                Opacity = 0.2;
                CloseColumnSpan = 1;
                IsPageUnavailableVisible = false;
                IsInvalid = true;
                RequestToTerminateStudy = true;
                IsPageEnabled = false;
                return;
            }

            TerminateStudyProcess();
        }

        private void TerminateStudyProcess()
        {
            var study = SampleRepo.GetItem(Utilities.StudyId);
            study.Completed = true;
            Utilities.IsCompleted = true;
            SampleRepo.SaveItem(study);
            AlarmNotificationService.DisableAlarmInDatabase();
            AlarmNotificationService.DisableAlarm();
            Utilities.Navigate(new ReportsPage());
        }

        void EditStudyDetails()
        {
            Utilities.Navigate(new EditActivitiesPage());
        }

        void NavigateToStudyMenu()
        {
            AlarmNotificationService.DisableAlarmInDatabase();
            AlarmNotificationService.DisableAlarm();
            Utilities.Navigate(new StudyMenuPage());
        }

        public void CloseActivitiesViewEvent()
        {
            Opacity = 1;
            ActivitiesVisible = false;
            IsPageEnabled = true;
        }

        public ICommand LongPress
        {
            get { return LongPressEvent(); }
        }

        Command LongPressEvent()
        {
            return new Command(item =>
            {
                ValidationText = $@"Do you want to undo the  observation for {OperatorName} from this round?";
                ShowOkCancel = true;
                IsOverrideVisible = false;
                ShowClose = false;
                Opacity = 0.2;
                CloseColumnSpan = 1;
                IsPageUnavailableVisible = false;
                IsInvalid = true;
                IsPageEnabled = false;
                RemoveObservationRequest = true;
                ActivitiesVisible = false;
            });
        }

        void ActivitySelectedEvent(object sender)
        {
            Rating = 0;
            var value = (int)sender;
            ActivityId = value;

            Observation.ActivityId = ActivityId;
            CurrentActivity = ActivityRepo.GetWithChildren(ActivityId);

            if (Utilities.RatedStudy && CurrentActivity.Rated)
            {
                Opacity = 0.2;
                RatingsVisible = true;
                IsPageEnabled = false;
            }
            else
            {
                Opacity = 1;
                ActivitiesVisible = false;
                IsPageEnabled = true;
                AddObservation();
            }

            ActivitiesVisible = false;
        }

        void RatingSelectedEvent(object sender)
        {
            var button = sender as Custom.CustomButton;
            Rating = button.Rating;

            Observation.Rating = Rating;

            AddObservation();

            Opacity = 1;
            RatingsVisible = false;
            IsPageEnabled = true;
        }

        void AddSelectedEvent(object sender)
        {
            var value = (int)sender;

            var existingObservation = ObservationRepo.GetItems()
                          .SingleOrDefault(x => x.OperatorId == value
                          && x.ObservationNumber == ObservationRound);

            //Activity = ActivityRepo.GetItem(value);
            //Comment = Activity.Comment;
            Opacity = 0.2;
            CommentsVisible = true;
            IsPageEnabled = false;
        }

        void SaveCommentDetails()
        {
            //if (Comment != null)
            //{
                //Activity.Comment = Comment.ToUpper();
                //ActivityRepo.SaveItem(Activity);
                //Utilities.ActivityPageHasUpdatedActivityChanges = true;
            //}

            Opacity = 1;
            CommentsVisible = false;
            //Comment = string.Empty;
            IsPageEnabled = true;
        }

        void CancelCommentDetails()
        {
            Opacity = 1;
            CommentsVisible = false;
            //Comment = string.Empty;
            IsPageEnabled = true;
        }

        static bool commentsVisible;
        public bool CommentsVisible
        {
            get => commentsVisible;
            set
            {
                commentsVisible = value;
                OnPropertyChanged();
            }
        }

        private void AddObservation()
        {
            var existingObservation = ObservationRepo.GetItems()
                                      .SingleOrDefault(x => x.OperatorId == operator1.Id
                                      && x.ObservationNumber == ObservationRound);

            if (existingObservation != null)
                Observation = existingObservation;

            Observation.ActivityName = CurrentActivity.ActivityName.Name;
            Observation.ActivityId = ActivityId;
            Observation.Rating = Rating;
            Observation.AliasActivityId = ActivityId;

            observations.Add(Observation);

            foreach (var item in observations)
            {
                item.StudyId = Utilities.StudyId;
                item.ObservationNumber = ObservationRound;
                ObservationRepo.SaveItem(item);
            }

            Operators = GetAllEnabledOperators();

            CreateOperatorObservations();

            Utilities.MainPageHasUpdatedObservationChanges = true;
        }

        private LimitsOfAccuracy LimitsOfAccuracyReached(Operator currentOperator)
        {
            var runningTotals = GetRunningTotals(currentOperator);

            foreach (var item in runningTotals)
            {
                TotalObservationsRequired = TotalObservationsRequired + item.ObservationsRequired;
                TotalObservationsTaken = TotalObservationsTaken + item.NumberOfObservations;
            }

            double totalPercentagePerOp = 0;
            if (TotalObservationsTaken > 0)
            {
                totalPercentagePerOp = Math.Ceiling((double)TotalObservationsTaken / TotalObservationsRequired * 100);
                var percentage = totalPercentagePerOp < 100 ? totalPercentagePerOp : 100;
                TotalOperatorPercentage = $"{percentage.ToString(CultureInfo.InvariantCulture)}%";
            }


            return new LimitsOfAccuracy()
            {
                AccuracyReached = totalPercentagePerOp >= 100,
                TotalPercentagePerOperator = totalPercentagePerOp
            };

        }

        public ICommand ItemClickedCommand
        {

            get { return ShowActivities(); }
        }

        public ICommand ItemActivityClickedCommand
        {

            get { return ShowRatings(); }
        }

        Command ShowActivities()
        {
            return new Command(item =>
            {
                operator1 = item as OperatorObservation;
                Observation = new Observation
                {
                    OperatorId = operator1.Id
                };
                OperatorName = operator1.Name;

                Opacity = 0.2;
                ActivitiesVisible = true;
                IsPageEnabled = false;
            });
        }

        Command ShowRatings()
        {
            return new Command((item) =>
            {
                Opacity = 0.2;
                RatingsVisible = true;
                ActivitiesVisible = false;
            });
        }

        void OverrideEvent(object sender)
        {
            if (RequestToTerminateStudy)
                TerminateStudyProcess();
            else if (RemoveObservationRequest)
                RemoveObservationForOperator();
            else
            {
                UpdateObservationRoundStatus();
                SetUpForNextObservationRound();
            }


            IsInvalid = false;
            Opacity = 1;
            IsPageEnabled = true;
        }

        private void GetObservationRound()
        {
            var lastObservationRound = ObservationRepo.GetItems().Where(x => x.StudyId == Utilities.StudyId).Distinct()
                                  .OrderByDescending(y => y.ObservationNumber)
                                  .Select(c => c.ObservationNumber).FirstOrDefault();

            if (lastObservationRound == 0)
            {
                ObservationRound = 1;
                if (ObservationRoundStatus?.Id == null)
                    SaveInitialObservationRoundStatus();
                return;
            }

            ObservationRoundStatus = ObservationRoundStatusRepo.GetItems()
                .FirstOrDefault(x => x.ObservationId == ObservationRound);

            var obsCount = ObservationRepo.GetItems()
                                          .Count(x => x.ObservationNumber == lastObservationRound
                                                && x.StudyId == Utilities.StudyId);

            if (ObservationRoundStatus?.Status == "Complete")
            {
                ObservationRound = lastObservationRound + 1;
                return;
            }

            var opsCount = GetAllEnabledOperators().Count();

            if (ObservationRound == lastObservationRound)
            {
                if (ObservationRoundStatus?.Status == "Complete")
                {
                    ObservationRound = lastObservationRound + 1;
                    return;
                }
                if (opsCount == obsCount)
                    ObservationRound = lastObservationRound;
            }
            else if (ObservationRound > lastObservationRound)
                ObservationRound = lastObservationRound + 1;
            else
            {
                ObservationRound = lastObservationRound + 1;
                SaveInitialObservationRoundStatus();
            }
        }

        private void SaveInitialObservationRoundStatus()
        {
            var id = ObservationRoundStatusRepo.SaveItem(new ObservationRoundStatus
            {
                ObservationId = ObservationRound,
                StudyId = Utilities.StudyId
            });

            ObservationRoundStatus = ObservationRoundStatusRepo.GetItem(id);
        }

        private bool IsStudyValid()
        {

            if (Utilities.StudyId == 0 || Utilities.IsCompleted)
                return false;

            if (Activities.Count == 0 || !Operators.Any())
            {
                InvalidText = $"Please add at least one operator to study {Utilities.StudyId.ToString()}";
                return false;
            }

            return true;
        }

        private void CreateOperatorObservations()
        {
            var ops = new ObservableCollection<OperatorObservation>();
            bool added = false;

            foreach (var item in Operators)
            {

                if (item.Observations.Count == 0) added = false;

                foreach (var obs in item.Observations)
                {
                    if (obs.ObservationNumber == ObservationRound)
                    {
                        var opObservation = new OperatorObservation
                        {
                            ActivityName = obs.ActivityName,
                            Rating = obs.Rating,
                            Name = item.Name,
                            Id = item.Id,
                            IsRated = obs.Rating > 0,
                            ObservedColour = System.Drawing.Color.Silver,
                        };

                        ops.Add(opObservation);
                        added = true;
                    }
                    else added = false;
                }

                if (added == false)
                {
                    var opObs = new OperatorObservation
                    {
                        Name = item.Name,
                        Id = item.Id,
                        IsRated = false,
                        ObservedColour = Xamarin.Forms.Color.FromHex("#E8EAEC")
                    };
                    ops.Add(opObs);
                }
            }

            OperatorObservations = ops;
            Utilities.OperatorObservations = ops;
            Utilities.AllObservationsTaken = AllObservationsTaken;
        }

        public ObservableCollection<Operator> GetAllEnabledOperators()
        {
            return new ObservableCollection<Operator>(OperatorRepo.GetAllWithChildren()
                                                          .Where(_ => _.StudyId == Utilities.StudyId
                                                           && _.IsEnabled).OrderBy(x => x.Id));
        }

        private ObservableCollection<Operator> GetAllEnabledAndDisabledOperators()
        {
            return new ObservableCollection<Operator>(OperatorRepo.GetAllWithChildren()
                                                          .Where(_ => _.StudyId == Utilities.StudyId
                                                           ));
        }

        private double GetStudyTotalPercent()
        {
            int totalObservationsTaken = 0;
            int totalObservationsRequired = 0;
            int maxObservationRound = 0;

            var observationsTaken = ObservationRepo.GetItems().Where(x => x.StudyId == Utilities.StudyId).ToList();

            totalObservationsTaken = observationsTaken.Count;

            if(totalObservationsTaken > 0)
            {
                maxObservationRound = observationsTaken
                                             .Distinct()
                                             .Max(x => x.ObservationNumber);
            }
           
            if (maxObservationRound < 10 || totalObservationsTaken < 10)
            {
                PercentagesVisible = false;
                TotalPercentageVisible = false;
                return 0;
            }
            else
            {
                PercentagesVisible = true;
                TotalPercentageVisible = true;
            }

            var activtyIds = observationsTaken.Select(x => new { Id = x.AliasActivityId })
                                              .Distinct().ToList();

            var distinctActivities = new List<dynamic>();

            foreach (var item in activtyIds)
            {
                distinctActivities.Add(item);
            }

            foreach (var item in distinctActivities)
            {
                var count = observationsTaken.Count(x => x.AliasActivityId == item.Id);
                double percentage = count > 0 ? (double)count / totalObservationsTaken : 0;

                var totalRequired = Utilities.CalculateObservationsRequired(percentage * 100);

                totalObservationsRequired = totalObservationsRequired + totalRequired;
            }

            double totalStudyPercent =  (double)totalObservationsTaken / totalObservationsRequired;

            return Math.Round(totalStudyPercent * 100, 1);
        }
    }
}
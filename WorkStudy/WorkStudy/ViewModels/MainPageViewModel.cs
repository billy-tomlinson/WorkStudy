using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using WorkStudy.Custom;
using WorkStudy.Model;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{

    public class MainPageViewModel : BaseViewModel
    {
        private OperatorObservation operator1;
        List<Observation> observations = new List<Observation>();

        public Command SaveObservations { get; set; }
        public Command ActivitySelected { get; set; }
        public Command RatingSelected { get; set; }
        public Command EndStudy { get; set; }
        public Command PauseStudy { get; set; }
        public Command EditStudy { get; set; }

        public MainPageViewModel(string conn) : base(conn)
        {
            ConstructorSetUp();
        }

        public MainPageViewModel()
        {
            ConstructorSetUp();
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


        static double totalPercent;
        public double TotalPercent
        {
            get => totalPercent;
            set
            {
                totalPercent = value;
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

        public void UpdateObservationRound()
        {
            ObservationRound = ObservationRound + 1;
        }

        public void SaveObservationDetails()
        {
            if (AllObservationsTaken)
            {
                Utilities.AllObservationsTaken = true;
                observations = new List<Observation>();
                UpdateObservationRound();
                CreateOperatorObservations();
                TotalPercent = GetStudyTotalPercent();
            }
            else
            {
                Utilities.AllObservationsTaken = false;
                ValidationText = "Not All Operators have been observed.";
                Opacity = 0.2;
                IsInvalid = true;
            }
        }

        void TerminateStudy()
        {
            var study = SampleRepo.GetItem(Utilities.StudyId);
            study.Completed = true;
            Utilities.IsCompleted = true;
            SampleRepo.SaveItem(study);
            Utilities.Navigate(new ReportsPage());
        }

        void EditStudyDetails()
        {
            Utilities.Navigate(new EditActivitiesPage());
        }

        void NavigateToStudyMenu()
        {
            Utilities.Navigate(new StudyMenuPage());
        }

        void ActivitySelectedEvent(object sender)
        {
            Rating = 0;
            ChangeButtonColour((int)sender);
            var value = (int)sender;
            ActivityId = value;

            Observation.ActivityId = ActivityId;
            CurrentActivity = ActivityRepo.GetItem(ActivityId);

            if (Utilities.RatedStudy && CurrentActivity.Rated)
            {
                Opacity = 0.2;
                RatingsVisible = true;
            }
            else
            {
                Opacity = 1;
                ActivitiesVisible = false;
                AddObservation();
            }

            ActivitiesVisible = false;
        }

        private void ChangeButtonColour(int sender)
        {
            IEnumerable<Activity> obsCollection = Activities;
            var list = new List<Activity>(obsCollection);
            var activity = list.Find(_ => _.Id == sender);
            activity.Colour = Utilities.UnClicked.GetHexString().Equals(activity.Colour.GetHexString())
                ? Utilities.Clicked : Utilities.UnClicked;
            list.RemoveAll(_ => _.Id == (int)sender);
            list.Add(activity);
            Activities = new ObservableCollection<Activity>(obsCollection);
            GroupActivities = Utilities.BuildGroupOfActivities(Activities);
        }

        void RatingSelectedEvent(object sender)
        {
            var button = sender as Custom.CustomButton;
            Rating = button.Rating;

            Observation.Rating = Rating;

            AddObservation();

            Opacity = 1;
            RatingsVisible = false;
        }

        private void AddObservation()
        {
            var existingObservation = ObservationRepo.GetItems()
                                      .SingleOrDefault(x => x.OperatorId == operator1.Id
                                      && x.ObservationNumber == ObservationRound);

            if (existingObservation != null)
                Observation = existingObservation;

            Observation.ActivityName = CurrentActivity.Name;
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
        }

        private LimitsOfAccuracy LimitsOfAccuracyReached(Operator currentOperator)
        {
            var runningTotals = GetRunningTotals(currentOperator);

            foreach (var item in runningTotals)
            {
                TotalObservationsRequired = TotalObservationsRequired + item.ObservationsRequired;
                TotalObservationsTaken = TotalObservationsTaken + item.NumberOfObservations;
            }

            double totalPercentage = 0;
            if(TotalObservationsTaken > 0)
            {
                totalPercentage = Math.Ceiling((double)TotalObservationsTaken / TotalObservationsRequired * 100);
                TotalOperatorPercentage = $"{totalPercentage.ToString(CultureInfo.InvariantCulture)}%";
            }


            return new LimitsOfAccuracy()
            {
                AccuracyReached = totalPercentage == 100,
                TotalPercentage = totalPercentage
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

                Activities = new ObservableCollection<Activity>(ActivityRepo.GetItems()
                                                                .Where(x => x.StudyId == Utilities.StudyId 
                                                                       && x.IsEnabled == true));
                GroupActivities = Utilities.BuildGroupOfActivities(Activities);
                Opacity = 0.2;
                ActivitiesVisible = true;
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

        private void ConstructorSetUp()
        {
            SaveObservations = new Command(SaveObservationDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);
            RatingSelected = new Command(RatingSelectedEvent);
            EndStudy = new Command(TerminateStudy);
            EditStudy = new Command(EditStudyDetails);
            PauseStudy = new Command(NavigateToStudyMenu);
            Opacity = 1.0;

            Operators = GetAllEnabledOperators();

            GetObservationRound();

            Activities = Get_Enabled_Activities();

            IsPageVisible = IsStudyValid();

            CreateOperatorObservations();

            TotalPercent = GetStudyTotalPercent();
        }

        private void GetObservationRound()
        {
            var lastObservationRound = ObservationRepo.GetItems().Where(x => x.StudyId == Utilities.StudyId).Distinct()
                                  .OrderByDescending(y => y.ObservationNumber)
                                  .Select(c => c.ObservationNumber).FirstOrDefault();
            
            var obsCount = ObservationRepo.GetItems().Count(x => x.ObservationNumber == lastObservationRound);
            var opsCount = GetAllEnabledAndDisabledOperators().Count();

            if (opsCount >= obsCount)
                ObservationRound = lastObservationRound;
            else
                ObservationRound = lastObservationRound + 1;
        }

        private bool IsStudyValid()
        {

            if (Utilities.StudyId == 0 || Utilities.IsCompleted)
                return false;

            if (Activities.Count == 0 || !Operators.Any())
            {
                InvalidText = $"Please add Activities and/or Operators to study {Utilities.StudyId.ToString()}";
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
                var limitsReached = LimitsOfAccuracyReached(item);

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
                            LimitsOfAccuracy = limitsReached.AccuracyReached,
                            TotalPercentageDouble = limitsReached.TotalPercentage,
                            TotalPercentage = limitsReached.TotalPercentage.ToString() + "%"
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
                        ObservedColour = System.Drawing.Color.Gray,
                        LimitsOfAccuracy = limitsReached.AccuracyReached,
                        TotalPercentageDouble = limitsReached.TotalPercentage,
                        TotalPercentage = limitsReached.TotalPercentage.ToString() + "%"
                    };
                    ops.Add(opObs);
                }
            }

            OperatorObservations = ops;
            Utilities.AllObservationsTaken = AllObservationsTaken;
        }

        private ObservableCollection<Operator> GetAllEnabledOperators()
        {
            return new ObservableCollection<Operator>(OperatorRepo.GetAllWithChildren()
                                                          .Where(_ => _.StudyId == Utilities.StudyId
                                                           && _.IsEnabled));
        }


        private ObservableCollection<Operator> GetAllEnabledAndDisabledOperators()
        {
            return new ObservableCollection<Operator>(OperatorRepo.GetAllWithChildren()
                                                          .Where(_ => _.StudyId == Utilities.StudyId
                                                           ));
        }
        private double GetStudyTotalPercent()
        {
            double totalPercent = 0;
            foreach (var op in OperatorObservations)
            {
                totalPercent = totalPercent + op.TotalPercentageDouble;
            }

            if (totalPercent > 0)
                totalPercent = totalPercent / OperatorObservations.Count();

            return Math.Round(totalPercent, 1);
        }
    }
}

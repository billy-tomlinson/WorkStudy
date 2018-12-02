using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private OperatorObservation operator1;
        List<Observation> Observations = new List<Observation>();
        bool isInvalid;

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

        public MainPageViewModel(bool isInvalid = false)
        {
            this.isInvalid = isInvalid;
            ConstructorSetUp();
        }

        private Observation Observation { get; set; }
        private int ActivityId { get; set; }
        private int Rating { get; set; }


        static ObservableCollection<Operator> operators;
        public ObservableCollection<Operator> Operators
        {
            get => operators;
            set
            {
                operators = value;
                OnPropertyChanged();
            }
        }

        static ObservableCollection<OperatorObservation> operatorObservations;
        public ObservableCollection<OperatorObservation> OperatorObservations
        {
            get => operatorObservations;
            set
            {
                operatorObservations = value;
                OnPropertyChanged();
            }
        }

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
                Observations = new List<Observation>();
                UpdateObservationRound();
                CreateOperatorObservations();
            }
            else 
            {
                Utilities.AllObservationsTaken = false;
                ValidationText = "Not All Operators have been observed.";
                IsInvalid = true;
            }
        }

        void TerminateStudy()
        {
            var study = SampleRepo.GetItem(Utilities.StudyId);
            study.Completed = true;
            SampleRepo.SaveItem(study);
            Utilities.Navigate(new ReportsPage());
        }

        void EditStudyDetails()
        {
            Utilities.Navigate(new EditActivities());
        }

        void NavigateToStudyMenu()
        {
            Utilities.Navigate(new StudyMenu());
        }

        void ActivitySelectedEvent(object sender)
        {
            ChangeButtonColour((int)sender);
            var value = (int)sender;
            ActivityId = value;

            Observation.ActivityId = ActivityId;
            CurrentActivity = ActivityRepo.GetItem(ActivityId);

            if (Utilities.RatedStudy && CurrentActivity.Rated)
                RatingsVisible = true;
            else
            {
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
            activity.Colour = System.Drawing.Color.Aquamarine.ToArgb().Equals(activity.Colour.ToArgb())
                ? System.Drawing.Color.BlueViolet : System.Drawing.Color.Aquamarine;
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

            RatingsVisible = false;
        }

        private void AddObservation()
        {
            var observation = Observations.Find(_ => _.OperatorId == operator1.Id);
            var exisitingObservation = ObservationRepo.GetItems()
                                      .SingleOrDefault(x => x.OperatorId == operator1.Id 
                                      && x.ObservationNumber == ObservationRound);
            
            if (exisitingObservation != null)
                Observation = exisitingObservation;

            Observation.ActivityName = CurrentActivity.Name;
            Observation.ActivityId = ActivityId;
            Observation.Rating = Rating;

            Observations.Add(Observation);

            foreach (var item in Observations)
            {
                item.StudyId = Utilities.StudyId;
                item.ObservationNumber = ObservationRound;
                ObservationRepo.SaveItem(item);
            }

            Operators = new ObservableCollection<Operator>(OperatorRepo.GetAllWithChildren()
                                              .Where(_ => _.StudyId == Utilities.StudyId));

            CreateOperatorObservations();    
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
            return new Command((item) =>
            {
                operator1 = item as OperatorObservation;
                Observation = new Observation();
                Observation.OperatorId = operator1.Id;
                OperatorName = operator1.Name;
                Activities = new ObservableCollection<Activity>(OperatorRepo.GetWithChildren(operator1.Id)
                                                                .Activities.ToList().Where(x => x.IsEnabled == true));
                GroupActivities = Utilities.BuildGroupOfActivities(Activities);
                ActivitiesVisible = true;
            });
        }

        Command ShowRatings()
        {
            return new Command((item) =>
            {
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

            Operators = new ObservableCollection<Operator>(OperatorRepo.GetAllWithChildren()
                                                          .Where(_ => _.StudyId == Utilities.StudyId));

            var lastObservation = ObservationRepo.GetItems().Where(x => x.StudyId == Utilities.StudyId).Distinct()
                                              .OrderByDescending(y => y.ObservationNumber)
                                              .Select(c => c.ObservationNumber).FirstOrDefault();

            if (!isInvalid)
                ObservationRound = lastObservation + 1;
            else
            {
                ObservationRound = lastObservation;
                ValidationText = "Not All Operators have been observed.";
            }
                
            Activities = Get_Enabled_Activities();

            IsPageVisible = IsStudyValid();

            CreateOperatorObservations();

            IsInvalid = false;

            if (isInvalid)
            {
                IsInvalid = true;
                SaveObservationDetails();
            }
                
        }

        private bool IsStudyValid()
        {

            if (Utilities.StudyId == 0 || Utilities.IsCompleted)
                return false;

            if ((Activities.Count == 0) ||
                    (!Operators.Any()) ||
                    (Operators.Any(_ => !_.Activities.Any(x => x.Rated))))
            {
                InvalidText = "Please add Activities and/or Operators to study.";
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
                    if(obs.ObservationNumber == ObservationRound)
                    {
                        var opObservation = new OperatorObservation
                        {
                            ActivityName = obs.ActivityName,
                            Rating = obs.Rating,
                            Name = item.Name,
                            Id = item.Id
                        };

                        ops.Add(opObservation);
                        added = true;
                    }
                    else added = false;
                }

                if(added == false)
                {
                    var opObs = new OperatorObservation
                    {
                        Name = item.Name,
                        Id = item.Id
                    };
                    ops.Add(opObs);
                } 
            }

            OperatorObservations = ops;
            Utilities.AllObservationsTaken = AllObservationsTaken;
        }
    }
}

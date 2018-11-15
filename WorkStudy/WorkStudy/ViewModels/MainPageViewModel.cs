using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private Operator oldOperator;
        private Operator operator1;
        public ObservableCollection<Operator> Operators { get; set; }
        public Command SaveObservations { get; set; }
        public Command ActivitySelected { get; set; }
        public Command RatingSelected { get; set; }
        readonly IBaseRepository<Operator> operatorRepo;
        readonly IBaseRepository<Observation> observationRepo;

        public MainPageViewModel()
        {
            SaveObservations = new Command(SaveObservationDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);
            RatingSelected = new Command(RatingSelectedEvent);
            operatorRepo = new BaseRepository<Operator>();
            observationRepo = new BaseRepository<Observation>();
            Operators = new ObservableCollection<Operator>(operatorRepo.GetItems());
        }

        ObservableCollection<Observation> observations;
        public ObservableCollection<Observation> Observations => Utilities.Observations;

        static int _studyNumber = 1;
        public int StudyNumber
        {
            get => _studyNumber;
            set
            {
                _studyNumber = value;
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

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private int activityId;
        public int ActivityId
        {
            get => activityId;
            set
            {
                activityId = value;
                OnPropertyChanged();
            }
        }


        private int rating;
        public int Rating
        {
            get => rating;
            set
            {
                rating = value;
                OnPropertyChanged();
            }
        }

        public void ShowOrHideOperators(Operator value)
        {
            value.Observed = "OBSERVED";

            if (oldOperator == value)
            {
                value.Isvisible = !value.Isvisible;
                UpdateOperators(value);
            }
            else
            {
                if (oldOperator != null)
                {
                    oldOperator.Isvisible = false;
                    UpdateOperators(oldOperator);

                }

                value.Isvisible = true;
                UpdateOperators(value);
            }

            oldOperator = value;
            oldOperator.Observed = "OBSERVED";
        }

        public void UpdateStudyNumber()
        {
            StudyNumber = StudyNumber + 1;
        }

        private void UpdateOperators(Operator value)
        {
            var index = Operators.IndexOf(value);
            Operators.Remove(value);
            Operators.Insert(index, value);
        }

        void SaveObservationDetails()
        {
            var observation = new Observation()
            {
                Date = DateTime.Now
                               
            };

            if (observations == null)
            {
                observations = new ObservableCollection<Observation>();
                observations = Utilities.Observations;
            }

            observations.Add(observation);
            Utilities.Observations = observations;
            UpdateStudyNumber();
            Utilities.Navigate(new MainPage());
        }

        void ActivitySelectedEvent(object sender)
        {
            var button = sender as Custom.CustomButton;
            ActivityId = button.ActivityId;
            RatingsVisible = true;
            ActivitiesVisible = false;
        }


        void RatingSelectedEvent(object sender)
        {
            var button = sender as Custom.CustomButton;
            Rating = button.Rating;
            RatingsVisible = false;
            ShowOrHideOperators(operator1);
        }

        public ICommand ItemClickedCommand
        {
            get { return ShowActivities(); }
        }

        Command ShowActivities()
        {
            return new Command((item) =>
            {
                operator1 = item as Operator;
                ActivitiesVisible = true;
                ShowOrHideOperators(operator1);
            });
        }
    }
}

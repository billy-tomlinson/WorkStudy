using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SQLiteNetExtensions.Extensions;
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
        List<Observation> Observations = new List<Observation>();


        public Command SaveObservations { get; set; }
        public Command ActivitySelected { get; set; }
        public Command RatingSelected { get; set; }
        public Command EndStudy { get; set; }

        readonly IBaseRepository<Operator> operatorRepo;
        readonly IBaseRepository<Observation> observationRepo;
        readonly IBaseRepository<Activity> activitiesRepo;

        public MainPageViewModel()
        {
            SaveObservations = new Command(SaveObservationDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);
            RatingSelected = new Command(RatingSelectedEvent);
            EndStudy = new Command(TerminateStudy);

            operatorRepo = new BaseRepository<Operator>();
            observationRepo = new BaseRepository<Observation>();
            activitiesRepo = new BaseRepository<Activity>();
            //Operators = new ObservableCollection<Operator>(operatorRepo.DatabaseConnection.GetAllWithChildren<Operator>());
            Operators = new ObservableCollection<Operator>(operatorRepo.GetItems());
            //Activities = new ObservableCollection<Activity>();
            Activities = new ObservableCollection<Activity>(activitiesRepo.GetItems());
        }

        private Observation Observation { get;set;}
        private int ActivityId { get; set; }
        private int Rating { get; set; }

        //public List<Activity> Activities 
        //{ 
        //    get
        //    {
        //        return (List<Activity>)activitiesRepo.GetItems();
        //        //return operatorRepo.DatabaseConnection.GetWithChildren<Operator>(operator1.Id).Activities;
               
        //    } 
        //}

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
            foreach (var item in Observations)
            {
                observationRepo.SaveItem(item);
            }

            var x = observationRepo.GetItems();

            Utilities.Navigate(new MainPage());
            UpdateStudyNumber();
        }


        void TerminateStudy()
        {
            Utilities.Navigate(new ReportsPage());
        }

        void ActivitySelectedEvent(object sender)
        {
            var value = (int)sender;
            ActivityId = value;

            Observation.ActivityId = ActivityId;

            RatingsVisible = true;
            ActivitiesVisible = false;
        }


        void RatingSelectedEvent(object sender)
        {
            var button = sender as Custom.CustomButton;
            Rating = button.Rating;

            Observation.Rating = Rating;
            Observations.Add(Observation);

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
                StudyNumber = 100;
                operator1 = item as Operator;
                Observation = new Observation();
                Observation.OperatorId = operator1.Id;
                Activities = new ObservableCollection<Activity>(operatorRepo.DatabaseConnection.GetWithChildren<Operator>(operator1.Id).Activities);
                ActivitiesVisible = true;
                ShowOrHideOperators(operator1);
            });
        }
    }
}

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

        public MainPageViewModel()
        {
            SaveObservations = new Command(SaveObservationDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);
            RatingSelected = new Command(RatingSelectedEvent);
            EndStudy = new Command(TerminateStudy);

            Operators = new ObservableCollection<Operator>(OperatorRepo.GetItems());
            Activities = new ObservableCollection<Activity>(ActivityRepo.GetItems());
        }

        private Observation Observation { get;set;}
        private int ActivityId { get; set; }
        private int Rating { get; set; }

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
                ObservationRepo.SaveItem(item);
            }

            Utilities.Navigate(new MainPage());
            UpdateStudyNumber();
        }


        void TerminateStudy()
        {
            Utilities.Navigate(new ReportsPage());
        }

        void ActivitySelectedEvent(object sender)
        {
            ChangeButtonColour((int)sender);
            var value = (int)sender;
            ActivityId = value;

            Observation.ActivityId = ActivityId;

            RatingsVisible = true;
            ActivitiesVisible = false;
        }

        private void ChangeButtonColour(int sender)
        {
            IEnumerable<Activity> obsCollection = Activities;
            var list = new List<Activity>(obsCollection);
            var activity = list.Find(_ => _.Id == sender);
            activity.Colour = System.Drawing.Color.Aquamarine.ToArgb().Equals(activity.Colour.ToArgb()) ? System.Drawing.Color.BlueViolet : System.Drawing.Color.Aquamarine;
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
            Observations.Add(Observation);

            RatingsVisible = false;
            ShowOrHideOperators(operator1);
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
                StudyNumber = 100;
                operator1 = item as Operator;
                Observation = new Observation();
                Observation.OperatorId = operator1.Id;
                OperatorName = operator1.Name;
                Activities = new ObservableCollection<Activity>(OperatorRepo.DatabaseConnection.GetWithChildren<Operator>(operator1.Id).Activities);
                GroupActivities = Utilities.BuildGroupOfActivities(Activities);
                ActivitiesVisible = true;
                ShowOrHideOperators(operator1);
            });
        }

        Command ShowRatings()
        {
            return new Command((item) =>
            {
                //var value = (int)sender;
                //ActivityId = value;
                //Observation.ActivityId = ActivityId;

                RatingsVisible = true;
                ActivitiesVisible = false;
            });
        }
    }
}

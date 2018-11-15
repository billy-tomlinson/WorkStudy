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
        public ObservableCollection<Operator> Operators { get; set; }
        public Command SaveObservations { get; set; }
        public Command ActivitySelected { get; set; }
        readonly IBaseRepository<Operator> operatorRepo;
        readonly IBaseRepository<Observation> observationRepo;

        public MainPageViewModel()
        {
            SaveObservations = new Command(SaveObservationDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);
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

        public void ShowOrHideOperators(Operator value)
        {
            if (oldOperator == value)
            {
                value.Isvisible = !value.Isvisible;
                value.Observed = "OBSERVED";
                UpdateOperators(value);
            }
            else
            {
                if (oldOperator != null)
                {
                    oldOperator.Isvisible = false;
                    oldOperator.Observed = "OBSERVED";
                    UpdateOperators(oldOperator);

                }

                value.Isvisible = true;
                value.Observed = "";
                UpdateOperators(value);
            }

            oldOperator = value;
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
        }

        void ActivitySelectedEvent(object sender)
        {
            var button = sender as Custom.CustomButton;
            ActivityId = button.ActivityId;
        }

        public ICommand ItemClickedCommand
        {
            get { return Navigate(); }
        }

        Command Navigate()
        {
            return new Command((item) =>
            {
                var operator1 = item as Operator;
                ActivitiesVisible = true;
                //ShowOrHideOperators(operator1);
            });
        }
    }
}

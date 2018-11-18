using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SQLiteNetExtensions.Extensions;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class Employee
    {
        public string Name
        {
            get;
            set;
        }
    }

    public class GroupOfEmployees
    {
        public Employee EmployeeOne
        {
            get;
            set;
        }
        public Employee EmployeeTwo
        {
            get;
            set;
        }

        public Employee EmployeeThree
        {
            get;
            set;
        }
    }

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
            Employees = new List<Employee>
            {
                new Employee{Name = "pilly"},
                new Employee{Name = "silly"},
                new Employee{Name = "dilly"},
                new Employee{Name = "filly"},
                new Employee{Name = "gilly"},
                new Employee{Name = "milly"}
            };

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
                Activities = new ObservableCollection<Activity>(operatorRepo.DatabaseConnection.GetWithChildren<Operator>(operator1.Id).Activities);
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

        private List<Employee> _employees;
        private List<GroupOfEmployees> _groupEmployees;


        public List<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }


        public List<GroupOfEmployees> GroupEmployees
        {
            get => BuildGroupOfEmployees();
            set
            {
                _groupEmployees = value;
                OnPropertyChanged();
            }
        }

        private List<GroupOfEmployees> BuildGroupOfEmployees()
        {
            int counter = 0;
            GroupOfEmployees groupOfEmployee = new GroupOfEmployees();
            var groupedEmployees = new List<GroupOfEmployees>();
            for (int i = 0; i < Employees.Count; i++)
            {

                if (counter == 0)
                {
                    groupOfEmployee.EmployeeOne = Employees[i];
                    counter++;
                }

                else if (counter == 1)
                {
                    groupOfEmployee.EmployeeTwo = Employees[i];
                    counter++;
                }

                else if (counter == 2)
                {
                    groupOfEmployee.EmployeeThree = Employees[i];
                    groupedEmployees.Add(groupOfEmployee);
                    groupOfEmployee = new GroupOfEmployees();
                    counter = 0;
                }


            }

            return groupedEmployees;
        }

    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AddOperatorsViewModel : BaseViewModel
    {
        public Command SaveOperator { get; set; }
        public Command SaveActivities { get; set; }
        public Command CancelActivities { get; set; }
        public Command ActivitySelected { get; set; }
        public Command ItemSelected { get; set; }
        public Command SettingsSelected { get; set; }
        public Command DeleteSelected { get; set; }
        public Command CloseRunningTotals { get; set; }

        public AddOperatorsViewModel()
        {
            ConstructorSetUp();
        }
        public AddOperatorsViewModel(string conn) : base(conn)
        {
            ConstructorSetUp();
        }

        private ObservableCollection<Operator> itemsCollection;
        public ObservableCollection<Operator> ItemsCollection
        {
            get => itemsCollection;
            set
            {
                itemsCollection = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<OperatorRunningTotal> runningTotals;
        public ObservableCollection<OperatorRunningTotal> RunningTotals
        {
            get => runningTotals;
            set
            {
                runningTotals = value;
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


        static bool runningTotalsVisible;
        public bool RunningTotalsVisible
        {
            get => runningTotalsVisible;
            set
            {
                runningTotalsVisible = value;
                OnPropertyChanged();
            }
        }

        void SaveOperatorDetails()
        {
            ValidateValues();

            if (!IsInvalid)
            {
                Name = Name.ToUpper().Trim();

                List<Operator> duplicatesCheck = new List<Operator>(ItemsCollection);

                if (duplicatesCheck.Find(_ => _.Name.ToUpper() == Name.ToUpper().Trim()) == null)
                    Operator.Id = OperatorRepo.SaveItem(new Operator() 
                    { 
                        Name = Name, 
                        IsEnabled = true
                    });

                ItemsCollection = GetAllOperators();

                Name = string.Empty;
            }
        }

        void DeleteSelectedEvent(object sender)
        {
            var value = (int)sender;

            Operator = OperatorRepo.GetItem(value);

            if(!StudyInProcess)
                OperatorRepo.DeleteItem(Operator); 
            else
            {
                if(Operator.Icon == "undo.png")
                {
                    Operator.Opacity = 1;
                    Operator.IsEnabled = true;
                    Operator.Icon = "delete.png";
                }
                else
                {
                    Operator.Opacity = 0.2;
                    Operator.IsEnabled = false;
                    Operator.Icon = "undo.png";
                }

                OperatorRepo.SaveItem(Operator);
            }
           
            ItemsCollection = GetAllOperators();
            Activities = Get_Rated_Enabled_Activities_WithChildren();
        }

        void OperatorSelectedEvent(object sender)
        {
            var value = (int)sender;
            Operator = OperatorRepo.GetWithChildren(value);
            RunningTotals = new ObservableCollection<OperatorRunningTotal>(GetRunningTotals(Operator));
            RunningTotalsVisible = true;
        }

        void CloseRunningTotalsEvent(object sender)
        {
            RunningTotalsVisible = false;
        }

        private void ConstructorSetUp()
        {
            SaveOperator = new Command(SaveOperatorDetails);
            ItemSelected = new Command(OperatorSelectedEvent);
            DeleteSelected = new Command(DeleteSelectedEvent);
            CloseRunningTotals = new Command(CloseRunningTotalsEvent);

            ItemsCollection = GetAllOperators();
            Activities = Get_Rated_Enabled_Activities_WithChildren();
            Operator = new Operator();
            Name = string.Empty;
        }

        private void ValidateValues()
        {
            ValidationText = "Please Enter a valid Name";
            Opacity = 0.2;
            IsInvalid = true;

            if ((Name != null && Name?.Trim().Length > 0))
            {
                Opacity = 1;
                IsInvalid = false;
            }    
        }

        public void ValidateOperatorActivities()
        {
            IsInvalid = true;
            Opacity = 0.2;

            var studyOperators = OperatorRepo.GetAllWithChildren()
                                          .Where(_ => _.StudyId == Utilities.StudyId).ToList();
            if (!studyOperators.Any())
            {
                ValidationText = "Add at least one operator.";
                return;
            }

            IsInvalid = false;
            Opacity = 1;
        }

        private ObservableCollection<Operator> GetAllOperators()
        {
            return new ObservableCollection<Operator>(OperatorRepo.GetAllWithChildren()
                       .Where(_ => _.StudyId == Utilities.StudyId));
        }
    }
}


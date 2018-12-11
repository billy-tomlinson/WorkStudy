using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WorkStudy.Model;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;
using WorkStudy.Custom;

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

        public Operator Operator;

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
                        IsEnabled = true,
                        ObservedColour = Utilities.InValidColour
                    });

                ItemsCollection = GetAllOperators();

                LinkOperatorToUnratedActivities();

                Name = string.Empty;
            }
        }

        void ActivitySelectedEvent(object sender)
        {
            var value = (int)sender;
            ChangeButtonColour(value);

            var exisitingActivity = Operator.Activities.Find(_ => _.Id == value);

            if (exisitingActivity == null)
            {
                var activity = ActivityRepo.GetItem(value);
                Operator.Activities.Add(activity);
            }
            else
            {
                Operator.Activities.Remove(exisitingActivity);
            }
        }

        void SaveActivityDetails()
        {
            if (Operator.Activities.Any(x => x.Rated))
                Operator.ObservedColour = Utilities.ValidColour;
            else
                Operator.ObservedColour = Utilities.InValidColour;
            
            OperatorRepo.UpdateWithChildren(Operator);
            ItemsCollection = GetAllOperators();
            ActivitiesVisible = false;
        }

        void CancelActivityDetails()
        {
            ActivitiesVisible = false;
        }

        public override void SubmitDetailsAndNavigate()
        {
            ValidateOperatorActivities();

            if (!IsInvalid)
            {
                LinkAllOperatorsToUnratedActivities();
                Utilities.Navigate(new StudyStartPage());
            }
        }

        public ICommand ItemClickedCommand
        {
            get { return ShowActivities(); }
        }

        private void ChangeButtonColour(int sender)
        {

            IEnumerable<Activity> obsCollection = Activities;
            var list = new List<Activity>(obsCollection);
            var activity = list.Find(_ => _.Id == sender);
            activity.Colour = Utilities.UnClicked.GetHexString().Equals(activity.Colour.GetHexString())
                ? Utilities.Clicked : Utilities.UnClicked;
            list.RemoveAll(_ => _.Id == sender);
            list.Add(activity);
            Activities = new ObservableCollection<Activity>(obsCollection);
            GroupActivities = Utilities.BuildGroupOfActivities(Activities);
        }

        public Command ShowActivities()
        {
            return new Command((item) =>
            {
                Operator = item as Operator;
                ChangeButtonColoursOnLoad();
                ActivitiesVisible = true;
            });
        }

        public void ChangeButtonColoursOnLoad()
        {
            IEnumerable<Activity> obsCollection = Activities;
            var list = new List<Activity>(obsCollection);
            var list1 = new List<Activity>(obsCollection);

            var operatorSpecific = Operator.Activities;

            foreach (var item in list)
            {
                list1.RemoveAll(_ => _.Id == (int)item.Id);
                item.Colour = Utilities.UnClicked;
                list1.Add(item);
            }

            foreach (var specific in operatorSpecific)
            {
                var activity = list1.Find(_ => _.Id == specific.Id);
                if (activity != null)
                {
                    activity.Colour = Utilities.Clicked;
                    list1.RemoveAll(_ => _.Id == (int)specific.Id);
                    list1.Add(activity);
                }
            }

            Activities = ConvertListToObservable(list1);
            GroupActivities = Utilities.BuildGroupOfActivities(Activities);
        }

        void AddActivitiesSelectedEvent(object sender)
        {
            var value = (int)sender;
            Operator = OperatorRepo.GetWithChildren(value);
            ChangeButtonColoursOnLoad();
            ActivitiesVisible = true;
        }

        void DeleteSelectedEvent(object sender)
        {
            var value = (int)sender;

            Operator = OperatorRepo.GetItem(value);

            if(!StudyInProcess)
            {
                var activities = OperatorActivityRepo.GetItems().Where(x => x.OperatorId == value);
                foreach (var item in activities)
                {
                    OperatorActivityRepo.DeleteItem(item);
                }

                OperatorRepo.DeleteItem(Operator); 
            }
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
        }

        private void ConstructorSetUp()
        {
            SaveOperator = new Command(SaveOperatorDetails);
            SaveActivities = new Command(SaveActivityDetails);
            CancelActivities = new Command(CancelActivityDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);
            ItemSelected = new Command(OperatorSelectedEvent);
            SettingsSelected = new Command(AddActivitiesSelectedEvent);
            DeleteSelected = new Command(DeleteSelectedEvent);

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

            if (studyOperators.Any(_ => !_.Activities.Any(x => x.Rated)))
            {
                ValidationText = "Some operators have no activities";
                return;
            }

            IsInvalid = false;
            Opacity = 1;
        }

        public void LinkAllOperatorsToUnratedActivities()
        {

            var ops = OperatorRepo.GetAllWithChildren()
                .Where(_ => _.StudyId == Utilities.StudyId);

            if (ops.Any(_ => _.Activities.Any(x => !x.Rated)))
                return;

            var activities = ActivityRepo.GetItems()
                .Where(x => x.IsEnabled && !x.Rated && x.StudyId == Utilities.StudyId);

            foreach (var op in ops)
            {
                var updatedOp = OperatorRepo.GetWithChildren(op.Id);

                foreach (var item in activities)
                {
                    updatedOp.Activities.Add(item);
                }

                OperatorRepo.UpdateWithChildren(updatedOp);
            }
        }

        public void LinkOperatorToUnratedActivities()
        {

            var op = OperatorRepo.GetWithChildren(Operator.Id);

            if (op.Activities.Any(x => !x.Rated))
                return;

            var activities = ActivityRepo.GetItems()
                .Where(x => x.IsEnabled && !x.Rated && x.StudyId == Utilities.StudyId);

            foreach (var item in activities)
            {
                op.Activities.Add(item);
            }

            OperatorRepo.UpdateWithChildren(op);
        }

        private ObservableCollection<Operator> GetAllOperators()
        {
            return new ObservableCollection<Operator>(OperatorRepo.GetAllWithChildren()
                       .Where(_ => _.StudyId == Utilities.StudyId));
        }
    }
}


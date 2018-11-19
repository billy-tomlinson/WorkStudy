using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SQLiteNetExtensions.Extensions;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AddOperatorsViewModel : BaseViewModel
    {
        readonly IBaseRepository<Operator> operatorRepo;
        readonly IBaseRepository<Activity> activitiesRepo;
        readonly IBaseRepository<OperatorActivity> operatorActivityRepo;
        readonly IBaseRepository<MergedActivities> mergedActivityRepo;
        public Command SaveOperator { get; set; }
        public Command SaveActivities { get; set; }
        public Command CancelActivities { get; set; }
        public Command ActivitySelected { get; set; }
        public Operator Operator;

        public AddOperatorsViewModel()
        {
            SaveOperator = new Command(SaveOperatorDetails);
            SaveActivities = new Command(SaveActivityDetails);
            CancelActivities = new Command(CancelActivityDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);

            operatorRepo = new BaseRepository<Operator>();
            activitiesRepo = new BaseRepository<Activity>();
            operatorActivityRepo = new BaseRepository<OperatorActivity>();
            mergedActivityRepo = new BaseRepository<MergedActivities>();

            Operators = new ObservableCollection<Operator>(operatorRepo.DatabaseConnection.GetAllWithChildren<Operator>());
            Activities = new ObservableCollection<Activity>(activitiesRepo.DatabaseConnection.GetAllWithChildren<Activity>());
            Operator = new Operator();
            Name = string.Empty;
        }


        public AddOperatorsViewModel(string conn)
        {
            SaveOperator = new Command(SaveOperatorDetails);
            SaveActivities = new Command(SaveActivityDetails);
            CancelActivities = new Command(CancelActivityDetails);
            ActivitySelected = new Command(ActivitySelectedEvent);

            operatorRepo = new BaseRepository<Operator>(conn);
            activitiesRepo = new BaseRepository<Activity>(conn);
            operatorActivityRepo = new BaseRepository<OperatorActivity>(conn);
            mergedActivityRepo = new BaseRepository<MergedActivities>(conn);

            Operators = new ObservableCollection<Operator>(operatorRepo.DatabaseConnection.GetAllWithChildren<Operator>());
            Activities = new ObservableCollection<Activity>(activitiesRepo.DatabaseConnection.GetAllWithChildren<Activity>());
            Operator = new Operator();
            Name = string.Empty;
        }

        private ObservableCollection<Operator> operators;
        public ObservableCollection<Operator> Operators
        {
            get => operators;
            set
            {
                operators = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Activity> activities;
        public ObservableCollection<Activity> Activities
        {
            get => activities;
            set
            {
                activities = value;
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
            List<Operator> duplicatesCheck = new List<Operator>(Operators);
            if (duplicatesCheck.Find(_ => _.Name.ToUpper() == Name.ToUpper().Trim()) == null)
                operatorRepo.SaveItem(new Operator { Name = Name.ToUpper().Trim() });
            Operators = new ObservableCollection<Operator>(operatorRepo.GetItems());
            Name = string.Empty;
        }

        void ActivitySelectedEvent(object sender)
        {
            var value = (int)sender;
            ChangeButtonColour(value);

            var exisitingActivity = Operator.Activities.Find(_ => _.Id == value);

            if(exisitingActivity == null)
            {
                var activity = activitiesRepo.GetItem(value);
                Operator.Activities.Add(activity);
            }
            else
            {
                Operator.Activities.Remove(exisitingActivity);
            }
        }

        void SaveActivityDetails()
        {
            operatorActivityRepo.DatabaseConnection.UpdateWithChildren(Operator);

            ActivitiesVisible = false;
        }

        void CancelActivityDetails()
        {
            ActivitiesVisible = false;
        }

        public override void SubmitDetailsAndNavigate()
        {
            Utilities.Navigate(new StudyStartPAge());
        }

        public ICommand ItemClickedCommand
        {
            get { return ShowActivities(); }
        }

        static ObservableCollection<MultipleActivities> _groupActivities;
        public ObservableCollection<MultipleActivities> GroupActivities
        {
            get => _groupActivities;
            set
            {
                _groupActivities = value;
                OnPropertyChanged();
            }
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
                item.Colour = System.Drawing.Color.Aquamarine;
                list1.Add(item);
            }

            foreach (var specific in operatorSpecific)
            {
                var activity = list1.Find(_ => _.Id == specific.Id);
                activity.Colour = System.Drawing.Color.BlueViolet;
                list1.RemoveAll(_ => _.Id == (int)specific.Id);
                list1.Add(activity);
            }

            Activities = new ObservableCollection<Activity>(list1.OrderBy(x => x.Name));
            GroupActivities = Utilities.BuildGroupOfActivities(Activities);
        }
    }
}


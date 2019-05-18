using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkStudy.Model;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AddActivitiesViewModel : BaseViewModel
    {
        public Command SaveActivity { get; set; }
        public Command SaveComment { get; set; }
        public Command CancelComment { get; set; }
        public Command ItemSelected { get; set; }
        public Command SettingsSelected { get; set; }
        public Command DeleteSelected { get; set; }
        public Command CloseCategories { get; set; }
        public Command SaveCategory { get; set; }
        public Activity Activity;

        public AddActivitiesViewModel()
        {
            ConstructorSetUp();
        }

        public AddActivitiesViewModel(string conn) : base(conn)
        {
            ConstructorSetUp();
        }

        static ObservableCollection<Activity> itemsCollection;
        public ObservableCollection<Activity> ItemsCollection
        {
            get => itemsCollection;
            set
            {
                itemsCollection = value;
                OnPropertyChanged();
            }
        }

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                comment = value;
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

        static bool commentsVisible;
        public bool CommentsVisible
        {
            get => commentsVisible;
            set
            {
                commentsVisible = value;
                OnPropertyChanged();
            }
        }

        static bool categoriesVisible;
        public bool CategoriesVisible
        {
            get => categoriesVisible;
            set
            {
                categoriesVisible = value;
                OnPropertyChanged();
            }
        }

        bool _isNonValueAdded;
        public bool IsNonValueAdded
        {
            get => _isNonValueAdded;
            set
            {
                _isNonValueAdded = value;
                OnPropertyChanged();
                Switch_Toggled();
            }
        }

        void Switch_Toggled()
        {
            ActivityType = IsNonValueAdded == false ?  "VALUE ADDED": "NON VALUE ADDED";
        }

        string activityType = "VALUE ADDED";
        public string ActivityType
        {
            get { return activityType; }
            set
            {
                activityType = value;
                OnPropertyChanged();
            }
        }

        void SaveActivityDetails()
        {
            ValidateValues();

            if (!IsInvalid)
            {
                var duplicatesCheck = ActivityNameRepo.GetItems()
                    .FirstOrDefault(_ => _.Name?.ToUpper() == Name.ToUpper().Trim());

                if (duplicatesCheck == null)
                {
                    var activityName = new ActivityName()
                    {
                        Name = Name.ToUpper().Trim()
                    };
                    var activity = new Activity
                    {
                        ActivityName = activityName,
                        IsEnabled = true,
                        Rated = true,
                        ObservedColour = Utilities.ValueAddedColour,
                        IsValueAdded = true
                    };

                    SaveActivityDetails(activity);
                    Utilities.ActivityPageHasUpdatedActivityChanges = true;
                }
                else
                {
                    ValidationText = $"{Name.ToUpper().Trim()} is a duplicate. Please add a unique activity or select from list.";
                    Opacity = 0.2;
                    IsInvalid = true;
                    ShowClose = true;
                    IsPageEnabled = false;
                }

                ItemsCollection = new ObservableCollection<Activity>(Get_All_Enabled_Activities().OrderByDescending(x => x.Id));

                HasElements = ItemsCollection.Count > 0;

                Name = string.Empty;
            }
        }

        void SaveCommentDetails()
        {
            if (Comment != null)
            {
                Activity.Comment = Comment.ToUpper();
                ActivityRepo.SaveItem(Activity);
                Utilities.ActivityPageHasUpdatedActivityChanges = true;
            }

            Opacity = 1;
            CommentsVisible = false;
            Comment = string.Empty;
            IsPageEnabled = true;
        }

        void CancelCommentDetails()
        {
            Opacity = 1;
            CommentsVisible = false;
            Comment = string.Empty;
            IsPageEnabled = true;
        }

        public override void SubmitDetailsAndNavigate()
        {
            ValidateActivitiesAdded();

            if (!IsInvalid)
            {
                Utilities.Navigate(new AddOperatorsPage());
            }
        }

        public ICommand ItemClickedCommand
        {
            get { return ShowComments(); }
        }

        Command ShowComments()
        {
            return new Command((item) =>
            {
                Activity = item as Activity;
                Comment = Activity.Comment;
                Opacity = 0.2;
                CommentsVisible = true;
                IsPageEnabled = false;
            });
        }

        public void ValidateValues()
        {
            ValidationText = "Please Enter a valid Name";
            IsPageEnabled = false;
            IsInvalid = true;
            ShowClose = true;
            Opacity = 0.2;

            if ((Name != null && Name?.Trim().Length > 0))
            {
                Opacity = 1;
                IsInvalid = false;
                IsPageEnabled = true;
            }
        }

        private void ValidateActivitiesAdded()
        {

            ValidationText = "Please add at least one Activity";
            IsPageEnabled = false;
            IsInvalid = true;
            ShowClose = true;
            Opacity = 0.2;

            var activities = Get_Rated_Enabled_Activities();

            if ((activities.Count > 0))
            {
                Opacity = 1;
                IsInvalid = false;
                IsPageEnabled = true;
            }
        }

        void CloseCategoriesEvent(object sender)
        {
            Opacity = 1.0;
            CategoriesVisible = false;
            IsPageEnabled = true;
        }

        void SaveCategoryEvent(object sender)
        {
            if(IsNonValueAdded)
            {
                Activity.ItemColour = Utilities.NonValueAddedColour;
                Activity.ObservedColour = Utilities.NonValueAddedColour;
            }
            else 
            {
                Activity.ItemColour = Utilities.ValueAddedColour;
                Activity.ObservedColour = Utilities.ValueAddedColour;
            }

            Activity.IsValueAdded = !IsNonValueAdded;
            ActivityRepo.SaveItem(Activity);
            Opacity = 1.0;
            IsPageEnabled = true;
            ItemsCollection = new ObservableCollection<Activity>(Get_All_Enabled_Activities()
                .OrderByDescending(x => x.Id));

            CategoriesVisible = false;
            Utilities.ActivityPageHasUpdatedActivityChanges = true;
        }

        void AddSelectedEvent(object sender)
        {
            var value = (int)sender;
            Activity = ActivityRepo.GetItem(value);
            Comment = Activity.Comment;
            Opacity = 0.2;
            CommentsVisible = true;
            IsPageEnabled = false;
        }

        async void DeleteSelectedEvent(object sender)
        {
            var value = (int)sender;

            if (!StudyInProcess)
               await DeleteActivity(value);
            else
            {
                var obs = ObservationRepo.GetItems().Where(x => x.ActivityId == value
                          && x.StudyId == Utilities.StudyId);

                var merged = MergedActivityRepo.GetItems().Where(x => x.ActivityId == value);

                if (obs.Any() || merged.Any())
                {
                    ValidationText = "Cannot delete an activity once used in Study.";
                    Opacity = 0.2;
                    IsInvalid = true;
                    IsPageEnabled = false;
                    ShowClose = true;
                }
                else
                    await DeleteActivity(value);
            }
            Utilities.ActivityPageHasUpdatedActivityChanges = true;
        }

        private async Task DeleteActivity(int value)
        {
            IsBusy = true;
            IsEnabled = false;
            Opacity = 0.2;
            Task deleteTask = Task.Run(() =>
            {
                Activity = ActivityRepo.GetWithChildren(value);
                ActivityRepo.DeleteItem(Activity);

                var activities = ActivityRepo
                                    .GetAllWithChildren()
                                    .Where(x => x.ActivityName.Name == Activity.ActivityName.Name
                                     && x.StudyId != Utilities.StudyId);

                if (!activities.Any())
                    ActivityNameRepo.DeleteItem(Activity.ActivityName);

                ItemsCollection = new ObservableCollection<Activity>(Get_All_Enabled_Activities().OrderByDescending(x => x.Id));
            });

            await deleteTask;

            HasElements = ItemsCollection.Count > 0;

            IsEnabled = true;
            IsBusy = false;
            Opacity = 1;
            IsPageEnabled = true;
        }

        void ActivitySelectedEvent(object sender)
        {
            var value = (int)sender;
            Activity = ActivityRepo.GetItem(value);
            IsNonValueAdded = !Activity.IsValueAdded;
            if(Activity.Rated)
            {
                Opacity = 0.2;
                CategoriesVisible = true;
                IsPageEnabled = false;
            }
        }

        private void ConstructorSetUp()
        {
            SaveActivity = new Command(SaveActivityDetails);
            SaveComment = new Command(SaveCommentDetails);
            CancelComment = new Command(CancelCommentDetails);
            ItemSelected = new Command(ActivitySelectedEvent);
            SettingsSelected = new Command(AddSelectedEvent);
            DeleteSelected = new Command(DeleteSelectedEvent);
            CloseCategories = new Command(CloseCategoriesEvent);
            SaveCategory = new Command(SaveCategoryEvent);

            Name = string.Empty;
            CheckActivitiesInUse();

            ItemsCollection = new ObservableCollection<Activity>(Get_All_Enabled_Activities().OrderByDescending(x => x.Id));

            HasElements = ItemsCollection.Count > 0;

            var count = ItemsCollection.Count;
            Activity = new Activity();
            Activity.SettingsIcon = Utilities.CommentsImage;
            IsPageEnabled = true;
        }

        private void CheckActivitiesInUse()
        {
            var activities = Get_All_Enabled_Activities();

            foreach (var item in activities)
            {
                var obs = ObservationRepo.GetItems()
                                         .Where(x => x.ActivityId == item.Id || x.AliasActivityId == item.Id
                                          && x.StudyId == Utilities.StudyId)
                                         .ToList();

                var merged = MergedActivityRepo.GetItems()
                                               .Where(x => x.ActivityId == item.Id || x.MergedActivityId == item.Id)
                                               .ToList();

                var deleteIcon = item.Rated ? Utilities.DeleteImage : string.Empty;

                if (obs.Any() || merged.Any())
                {
                    deleteIcon = string.Empty;
                }

                var activity = ActivityRepo.GetWithChildren(item.Id);
                activity.DeleteIcon = deleteIcon;

                SaveActivityDetails(activity);
                Utilities.ActivityPageHasUpdatedActivityChanges = true;
            }
        }
    }
}



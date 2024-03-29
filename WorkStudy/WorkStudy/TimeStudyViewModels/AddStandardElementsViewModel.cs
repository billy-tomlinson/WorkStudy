﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeStudy.Model;
using TimeStudy.Pages;
using TimeStudy.Services;
using Xamarin.Forms;

namespace TimeStudy.ViewModels
{
    public class AddStandardElementsViewModel : BaseViewModel
    {
        public Command SaveActivity { get; set; }
        public Command SaveComment { get; set; }
        public Command CancelComment { get; set; }
        public Command ItemSelected { get; set; }
        public Command MoveUpSelected { get; set; }
        public Command MoveDownSelected { get; set; }
        public Command CloseCategories { get; set; }
        public Command SaveCategory { get; set; }
        public Command SettingsSelected { get; set; }
        public Command DeleteSelected { get; set; }
        public WorkElement Activity;
        public int ActivitiesCount;
       
        public AddStandardElementsViewModel()
        {
            ConstructorSetUp();
        }

        public AddStandardElementsViewModel(string conn) : base(conn)
        {
            ConstructorSetUp();
        }

        static ObservableCollection<WorkElement> itemsCollection;
        public ObservableCollection<WorkElement> ItemsCollection
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
            ActivityType = IsNonValueAdded == false ? "STANDARD" : "OCCASIONAL";
        }

        string activityType = "STANDARD";
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
                var duplicatesCheck = WorkElementRepo.GetAllWithChildren()
                    .Where(x => x.StudyId == Utilities.StudyId)
                    .FirstOrDefault(_ => _.Name?.ToUpper() == Name.ToUpper().Trim());

                if (duplicatesCheck == null)
                {
                    var activities = WorkElementRepo.GetAllWithChildren().Where(x => x.StudyId == Utilities.StudyId).Count();

                    var activityName = new WorkElementName()
                    {
                        Name = Name.ToUpper().Trim()
                    };
                    var activity = new WorkElement
                    {
                        ActivityName = activityName,
                        IsEnabled = true,
                        Rated = true,
                        ObservedColour = Utilities.ValueAddedColour,
                        IsValueAdded = true,
                        Sequence = activities + 1
                    };

                    SaveWorkElementDetails(activity);
                    Utilities.StandardElementPageHasUpdatedStandardElementChanges = true;
                }
                else
                {
                    ValidationText = $"{Name.ToUpper().Trim()} is a duplicate. Please add a unique element.";
                    Opacity = 0.2;
                    IsInvalid = true;
                    ShowClose = true;
                }

                SetElementsColour();
                ItemsCollection = new ObservableCollection<WorkElement>(WorkElementRepo.GetAllWithChildren().Where(x => x.StudyId == Utilities.StudyId)
                    .OrderByDescending(x => x.Id));

                HasElements = ItemsCollection.Count > 0;

                ActivitiesCount = ItemsCollection.Count;
                Name = string.Empty;
            }
        }

        void SaveCommentDetails()
        {
            if (Comment != null)
            {
                Activity.Comment = Comment.ToUpper();
                WorkElementRepo.SaveItem(Activity);
                Utilities.StandardElementPageHasUpdatedStandardElementChanges = true;
            }

            Opacity = 1;
            CommentsVisible = false;
            Comment = string.Empty;
        }

        void CancelCommentDetails()
        {
            Opacity = 1;
            CommentsVisible = false;
            Comment = string.Empty;
        }

        public override void SubmitDetailsAndNavigate()
        {
            ValidateActivitiesAdded();

            if (!IsInvalid)
            {
                Utilities.Navigate(new AddForeignElementsPage());
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
                Activity = item as WorkElement;
                Comment = Activity.Comment;
                Opacity = 0.2;
                CommentsVisible = true;
            });
        }

        public void ValidateValues()
        {
            ValidationText = "Please Enter a valid Name";

            IsInvalid = true;
            ShowClose = true;
            Opacity = 0.2;
            IsPageEnabled = false;

            if ((Name != null && Name?.Trim().Length > 0))
            {
                if (Name == "1" || Name.ToUpper() == "I")
                    Name = Name + ".";

                Opacity = 1;
                IsInvalid = false;
                IsPageEnabled = true;
            }
        }

        private void ValidateActivitiesAdded()
        {

            ValidationText = "Please add at least one Element";

            IsInvalid = true;
            ShowClose = true;
            Opacity = 0.2;

            var activities = Get_Rated_Enabled_WorkElements();

            if ((activities.Count > 0))
            {
                Opacity = 1;
                IsInvalid = false;
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
            if (IsNonValueAdded)
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
            WorkElementRepo.SaveItem(Activity);
            Opacity = 1.0;
            IsPageEnabled = true;
            ItemsCollection = new ObservableCollection<WorkElement>(WorkElementRepo.GetAllWithChildren().Where(x => x.StudyId == Utilities.StudyId)
                .OrderByDescending(x => x.Id));

            CategoriesVisible = false;
            Utilities.StandardElementPageHasUpdatedStandardElementChanges = true;
        }

        void ActivitySelectedEvent(object sender)
        {
            var value = (int)sender;
            Activity = WorkElementRepo.GetItem(value);
            IsNonValueAdded = !Activity.IsValueAdded;
            if (Activity.Rated)
            {
                Opacity = 0.2;
                CategoriesVisible = true;
                IsPageEnabled = false;
            }
        }

        private void SetElementsColour()
        {
            var activities = Get_All_NonValueAdded_Enabled_Unrated_WorkElements();

            foreach (var item in activities)
            {
                item.ItemColour = Utilities.InactiveColour;
                item.ObservedColour = Utilities.InactiveColour;
                WorkElementRepo.SaveItem(item);
            }

            activities = Get_All_NonValueAdded_Enabled_Rated_WorkElements();

            foreach (var item in activities)
            {
                item.ItemColour = Utilities.NonValueAddedColour;
                item.ObservedColour = Utilities.NonValueAddedColour;
                WorkElementRepo.SaveItem(item);
            }
        }

        void AddSelectedEvent(object sender)
        {
            var value = (int)sender;
            Activity = WorkElementRepo.GetItem(value);
            Comment = Activity.Comment;
            Opacity = 0.2;
            CommentsVisible = true;
        }

        void SetAllActivitiesBackToEnabled()
        {
            if (StudyInProcess) return;

            var activities = WorkElementRepo.GetAllWithChildren().Where(x => x.StudyId == Utilities.StudyId);
            foreach (var item in activities)
            {
                if (item.Rated)
                {
                    item.Opacity = 1;
                    item.IsEnabled = true;
                    item.DeleteIcon = Utilities.DeleteImage;
                    WorkElementRepo.SaveItem(item);
                }
            }
        }

        async void DeleteSelectedEvent(object sender)
        {
            var value = (int)sender;

            Activity = WorkElementRepo.GetItem(value);

            if (!StudyInProcess)
                await DeleteActivity(value);
            else
            {
                if (Activity.DeleteIcon == Utilities.UndoImage)
                {
                    Activity.Opacity = 1;
                    Activity.IsEnabled = true;
                    Activity.DeleteIcon = Utilities.DeleteImage;
                }
                else
                {
                    Activity.Opacity = 0.2;
                    Activity.IsEnabled = false;
                    Activity.DeleteIcon = Utilities.UndoImage;
                }

                WorkElementRepo.SaveItem(Activity);
            }

            SetElementsColour();
            ItemsCollection = new ObservableCollection<WorkElement>(WorkElementRepo.GetAllWithChildren().Where(x => x.StudyId == Utilities.StudyId)
                .OrderByDescending(x => x.Id));

            HasElements = ItemsCollection.Count > 0;

            Utilities.StandardElementPageHasUpdatedStandardElementChanges = true;
        }

        private async Task DeleteActivity(int value)
        {
            IsBusy = true;
            IsEnabled = false;
            Opacity = 0.2;
            Task deleteTask = Task.Run(() =>
            {
                Activity = WorkElementRepo.GetWithChildren(value);

                WorkElementRepo.DeleteItem(Activity);

                WorkElementNameRepo.DeleteItem(Activity.ActivityName);

                SetElementsColour();
                ItemsCollection = new ObservableCollection<WorkElement>(WorkElementRepo.GetAllWithChildren().Where(x => x.StudyId == Utilities.StudyId)
                    .OrderByDescending(x => x.Id));
                ActivitiesCount = ItemsCollection.Count;
            });

            await deleteTask;

            HasElements = ItemsCollection.Count > 0;

            IsEnabled = true;
            IsBusy = false;
            Opacity = 1;
        }

        private void ConstructorSetUp()
        {
            SaveActivity = new Command(SaveActivityDetails);
            SaveComment = new Command(SaveCommentDetails);
            CancelComment = new Command(CancelCommentDetails);
            ItemSelected = new Command(ActivitySelectedEvent);
            CloseCategories = new Command(CloseCategoriesEvent);
            SaveCategory = new Command(SaveCategoryEvent);
            SettingsSelected = new Command(AddSelectedEvent);
            DeleteSelected = new Command(DeleteSelectedEvent);

            IsPageVisible = IsStudyValid();

            Name = string.Empty;
            CheckActivitiesInUse();
            SetElementsColour();
            SetAllActivitiesBackToEnabled();
            ItemsCollection = new ObservableCollection<WorkElement>(WorkElementRepo.GetAllWithChildren().Where(x => x.StudyId == Utilities.StudyId)
                .OrderByDescending(x => x.Id));
            ActivitiesCount = ItemsCollection.Count;

            HasElements = ItemsCollection.Count > 0;

            var count = ItemsCollection.Count;
            Activity = new WorkElement
            {
                SettingsIcon = Utilities.CommentsImage
            };
        }

        private bool IsStudyValid()
        {

            if (Utilities.StudyId == 0 || Utilities.IsCompleted)
                return false;

            return true;
        }

        private void CheckActivitiesInUse()
        {
            var activities = Get_All_Enabled_WorkElements();

            foreach (var item in activities)
            {
                var deleteIcon = item.Rated ? Utilities.DeleteImage : string.Empty;

                var activity = WorkElementRepo.GetWithChildren(item.Id);
                activity.DeleteIcon = deleteIcon;

                SaveWorkElementDetails(activity);
                Utilities.StandardElementPageHasUpdatedStandardElementChanges = true;
            }
        }
    }
}



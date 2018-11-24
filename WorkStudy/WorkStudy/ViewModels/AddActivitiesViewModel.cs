using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AddActivitiesViewModel : BaseViewModel
    {
        public Command SaveActivity { get; set; }
        public Command SaveComment { get; set; }
        public Command CancelComment { get; set; }
        public Activity Activity;

        public AddActivitiesViewModel()
        {
            
            ConstructorSetUp();
        }

        public AddActivitiesViewModel(string conn) : base(conn)
        {
            ConstructorSetUp();
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

        void SaveActivityDetails()
        {
            ValidateValues();

            if (!IsInvalid)
            {
                List<Activity> duplicatesCheck = new List<Activity>(Activities);
                if (duplicatesCheck.Find(_ => _.Name.ToUpper() == Name.ToUpper().Trim()) == null)
                    ActivityRepo.SaveItem(new Activity { Name = Name.ToUpper().Trim(), IsEnabled = true });
                Activities = GetEnabledActivities();

                Name = string.Empty;
            }           
        }

        void SaveCommentDetails()
        {
            if (Comment != null)
            {
                Activity.Comment = Comment.ToUpper();
                ActivityRepo.SaveItem(Activity);
            }
           
            CommentsVisible = false;
            Comment = string.Empty;
        }

        void CancelCommentDetails()
        {
            CommentsVisible = false;
            Comment = string.Empty;
        }

        public override void SubmitDetailsAndNavigate()
        {
            ValidateActivitiesAdded();

            if (!IsInvalid)
            {
                Utilities.Navigate(new AddOperators());
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
                CommentsVisible = true;
            });
        }

        public void ValidateValues()
        {
            ValidationText = "Please Enter a valid Name";

            IsInvalid = true;

            if ((Name != null && Name?.Trim().Length > 0))
                IsInvalid = false;
        }

        private void ValidateActivitiesAdded()
        {
            
            ValidationText = "Please add at least one Activity";

            IsInvalid = true;

            var activities = GetEnabledActivities();

            if ((activities.Count > 0))
                IsInvalid = false;
        }

        private void ConstructorSetUp()
        {
            SaveActivity = new Command(SaveActivityDetails);
            SaveComment = new Command(SaveCommentDetails);
            CancelComment = new Command(CancelCommentDetails);

            Name = string.Empty;
            Activities = GetEnabledActivities();
            Activity = new Activity();
        }
    }
}


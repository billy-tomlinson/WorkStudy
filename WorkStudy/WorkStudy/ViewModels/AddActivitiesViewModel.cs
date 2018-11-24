using System.Collections.Generic;
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
        public Command CloseView { get; set; }
        public Activity Activity;
        public string ValidationText => "Please Enter a valid Name";

        public AddActivitiesViewModel()
        {
            
            ConstructorSetUp();
        }

        public AddActivitiesViewModel(string conn) : base(conn)
        {
            ConstructorSetUp();
        }

        private void ConstructorSetUp()
        {
            SaveActivity = new Command(SaveActivityDetails);
            SaveComment = new Command(SaveCommentDetails);
            CancelComment = new Command(CancelCommentDetails);
            CloseView = new Command(CloseValidationView);

            Name = string.Empty;
            Activities = GetEnabledActivities();
            Activity = new Activity();
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

        bool isInvalid = false;
        public bool IsInvalid
        {
            get { return isInvalid; }
            set
            {
                isInvalid = value;
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
            Utilities.Navigate(new AddOperators());
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

        private void ValidateValues()
        {
            IsInvalid = true;

            if((Name != null && Name?.Trim().Length > 0)) 
                IsInvalid = false;
        }

        public void CloseValidationView()
        {
            IsInvalid = false;
        }
    }
}


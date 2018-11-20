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
            SaveActivity = new Command(SaveActivityDetails);
            SaveComment = new Command(SaveCommentDetails);
            CancelComment = new Command(CancelCommentDetails);
            Name = string.Empty;
            Activities = new ObservableCollection<Activity>(ActivityRepo.GetItems());
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
            List<Activity> duplicatesCheck = new List<Activity>(Activities);
            if(duplicatesCheck.Find(_ => _.Name.ToUpper() == Name.ToUpper().Trim()) == null)
                ActivityRepo.SaveItem(new Activity { Name = Name.ToUpper().Trim(), IsEnabled = true});
            Activities = new ObservableCollection<Activity>(ActivityRepo.GetItems());

            Name = string.Empty;
        }

        void SaveCommentDetails()
        {
            Activity.Comment = Comment.ToUpper();
            ActivityRepo.SaveItem(Activity);

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
    }
}


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
        readonly IBaseRepository<Activity> activityRepo;
        public Command SaveActivity { get; set; }
        public Command SaveComment { get; set; }
        public Command CancelComment { get; set; }
        public static string ActivityName;
        List<Activity> AddedActivities = new List<Activity>();

        public AddActivitiesViewModel()
        {
            SaveActivity = new Command(SaveActivityDetails);
            SaveComment = new Command(SaveCommentDetails);
            CancelComment = new Command(CancelCommentDetails);
            activityRepo = new BaseRepository<Activity>();
            Name = string.Empty;
        }

        ObservableCollection<string> activities;
        public ObservableCollection<string> Activities => Utilities.Activities;

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
            if (activities == null)
            {
                activities = new ObservableCollection<string>();
                activities = Utilities.Activities;
            }

            if(!activities.Contains(Name.ToUpper().Trim()))
                activities.Add(Name.ToUpper());
            
            Utilities.Activities = activities;

            AddedActivities.Add(new Activity { Name = Name.ToUpper() });

            Name = string.Empty;
        }

        void SaveCommentDetails()
        {
            var index  = AddedActivities.FindIndex(_ => _.Name == ActivityName);

            AddedActivities.RemoveAt(index);
            AddedActivities.Add(new Activity { Name = ActivityName, Comment = Comment });

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
            foreach (var element in AddedActivities)
            {
                activityRepo.SaveItem(element);
            }

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
                ActivityName = (string)item;
                var activity = AddedActivities.Find(_ => _.Name == ActivityName);
                Comment = activity?.Comment;
                CommentsVisible = true;
            });
        }
    }
}


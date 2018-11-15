using System.Collections.ObjectModel;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AddActivitiesViewModel : BaseViewModel
    {
        readonly IBaseRepository<Activity> activityRepo;
        public Command SaveActivity { get; set; }
        public AddActivitiesViewModel()
        {
            SaveActivity = new Command(SaveActivityDetails);
            activityRepo = new BaseRepository<Activity>(App.DatabasePath);
            Comment = string.Empty;
        }

        ObservableCollection<string> activities;
        public ObservableCollection<string> Activities => Utilities.Comments;

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
        void SaveActivityDetails()
        {
            if (activities == null)
            {
                activities = new ObservableCollection<string>();
                activities = Utilities.Comments;
            }

            activities.Add(Comment);
            Utilities.Comments = activities;
            Comment = string.Empty;
        }

        public override void SubmitDetailsAndNavigate()
        {
            foreach (var element in Activities)
            {
                var activity = new Activity { Name = element };
                activityRepo.SaveItem(activity);
            }

            Utilities.Navigate(new AddOperators());
        }
    }
}


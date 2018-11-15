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

        void SaveActivityDetails()
        {
            if (activities == null)
            {
                activities = new ObservableCollection<string>();
                activities = Utilities.Activities;
            }

            activities.Add(Name);
            Utilities.Activities = activities;
            Name = string.Empty;
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


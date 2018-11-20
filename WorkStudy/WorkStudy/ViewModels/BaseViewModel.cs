using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
       
        public event PropertyChangedEventHandler PropertyChanged;


        public BaseViewModel()
        {
            SubmitDetails = new Command(SubmitDetailsAndNavigate);
        }


        public Command SubmitDetails { get; set; }

        public IBaseRepository<Operator> OperatorRepo => new BaseRepository<Operator>();

        public IBaseRepository<Observation> ObservationRepo => new BaseRepository<Observation>();

        public IBaseRepository<Activity> ActivityRepo  => new BaseRepository<Activity>();

        public IBaseRepository<MergedActivities> MergedActivityRepo => new BaseRepository<MergedActivities>();

        public IBaseRepository<OperatorActivity> OperatorActivityRepo => new BaseRepository<OperatorActivity>();

        public IBaseRepository<ActivitySampleStudy> SampleRepo => new BaseRepository<ActivitySampleStudy>();

        static ObservableCollection<Activity> activities;
        public ObservableCollection<Activity> Activities
        {
            get => activities;
            set
            {
                activities = value;
                OnPropertyChanged();
            }
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
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SubmitDetailsAndNavigate(){}

        public ObservableCollection<Activity> GetEnabledActivities()
        {
            return new ObservableCollection<Activity>(ActivityRepo.GetItems().Where(x => x.IsEnabled == true));
        }
    }
}

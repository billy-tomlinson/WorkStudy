using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
       
        public event PropertyChangedEventHandler PropertyChanged;

        public IBaseRepository<Operator> operatorRepo;
        public IBaseRepository<Observation> observationRepo;
        public IBaseRepository<Activity> activitiesRepo;

        public BaseViewModel()
        {
            SubmitDetails = new Command(SubmitDetailsAndNavigate);
        }

        public Command SubmitDetails { get; set; }

        public IBaseRepository<Operator> OperatorRepo => new BaseRepository<Operator>();

        public IBaseRepository<Observation> ObservationRepo => new BaseRepository<Observation>();

        public IBaseRepository<Activity> ActivitiesRepo  => new BaseRepository<Activity>();

        public IBaseRepository<MergedActivities> MergedActivityRepo => new BaseRepository<MergedActivities>();

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SubmitDetailsAndNavigate(){}
    }
}

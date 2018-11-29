using System.Collections.Generic;
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
        public Command CloseView { get; set; }
        private readonly string conn;

        public BaseViewModel(string conn = null)
        {
            this.conn = conn;
            SubmitDetails = new Command(SubmitDetailsAndNavigate);
            CloseView = new Command(CloseValidationView);
            EnsureTableCreation();
            IsPageVisible = (Utilities.StudyId > 0 && !Utilities.IsCompleted);
        }

        public Command SubmitDetails { get; set; }

        public IBaseRepository<Operator> OperatorRepo => new BaseRepository<Operator>(conn);

        public IBaseRepository<Observation> ObservationRepo => new BaseRepository<Observation>(conn);

        public IBaseRepository<Activity> ActivityRepo  => new BaseRepository<Activity>(conn);

        public IBaseRepository<MergedActivities> MergedActivityRepo => new BaseRepository<MergedActivities>(conn);

        public IBaseRepository<OperatorActivity> OperatorActivityRepo => new BaseRepository<OperatorActivity>(conn);

        public IBaseRepository<ActivitySampleStudy> SampleRepo => new BaseRepository<ActivitySampleStudy>(conn);

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


        bool isPageVisible = false;
        public bool IsPageVisible
        {
            get { return isPageVisible; }
            set
            {
                isPageVisible = value;
                OnPropertyChanged();
            }
        }


        private string validationText;
        public string ValidationText
        {
            get => validationText;
            set
            {
                validationText = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SubmitDetailsAndNavigate(){}

        public ObservableCollection<Activity> Get_Rated_Enabled_Activities()
        {
            return new ObservableCollection<Activity>(ActivityRepo.GetItems()
                                         .Where(x => x.IsEnabled && x.Rated && x.StudyId == Utilities.StudyId));
        }

        public ObservableCollection<Activity> Get_Enabled_Activities()
        {
            return new ObservableCollection<Activity>(ActivityRepo.GetItems()
                .Where(x => x.IsEnabled && x.StudyId == Utilities.StudyId));
        }

        public ObservableCollection<Activity> Get_Rated_Enabled_Activities_WithChildren()
        {
            return new ObservableCollection<Activity>(ActivityRepo.GetAllWithChildren()
                                        .Where(x => x.IsEnabled && x.Rated && x.StudyId == Utilities.StudyId));
        }

        public ObservableCollection<Activity> ConvertListToObservable(List<Activity> list1)
        {
            return new ObservableCollection<Activity>(list1.OrderBy(x => x.Id).Where(x => x.IsEnabled));
        }

        private void EnsureTableCreation()
        {
            OperatorRepo.DatabaseConnection.CreateTable<Operator>();
            ObservationRepo.DatabaseConnection.CreateTable<Observation>();
            ActivityRepo.DatabaseConnection.CreateTable<Activity>();
            MergedActivityRepo.DatabaseConnection.CreateTable<MergedActivities>();
            OperatorActivityRepo.DatabaseConnection.CreateTable<ActivitySampleStudy>();
            SampleRepo.DatabaseConnection.CreateTable<OperatorActivity>();
        }

        public void CloseValidationView()
        {
            IsInvalid = false;
        }
    }
}

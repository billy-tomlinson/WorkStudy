using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WorkStudy.Model;
using WorkStudy.Pages;
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
            InvalidText = "Please create a new study or select an existing one.";
            IsPageVisible = (Utilities.StudyId > 0 && !Utilities.IsCompleted);
            if (!Utilities.OnStudyPage) 
            {
                IsOkToNavigateAway();
            }
        }

        public Command SubmitDetails { get; set; }

        public IBaseRepository<Operator> OperatorRepo => new BaseRepository<Operator>(conn);

        public IBaseRepository<Observation> ObservationRepo => new BaseRepository<Observation>(conn);

        public IBaseRepository<Activity> ActivityRepo  => new BaseRepository<Activity>(conn);

        public IBaseRepository<MergedActivities> MergedActivityRepo => new BaseRepository<MergedActivities>(conn);

        public IBaseRepository<OperatorActivity> OperatorActivityRepo => new BaseRepository<OperatorActivity>(conn);

        public IBaseRepository<ActivitySampleStudy> SampleRepo => new BaseRepository<ActivitySampleStudy>(conn);

        public TimeSpan CurrentTime => DateTime.Now.TimeOfDay;

        static int _observationRound;
        public int ObservationRound
        {
            get => _observationRound;
            set
            {
                _observationRound = value;
                OnPropertyChanged();
            }
        }

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


        double opacity = 1;
        public double Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                OnPropertyChanged();
            }
        }

        string invalidText;
        public string InvalidText
        {
            get { return invalidText; }
            set
            {
                invalidText = value;
                OnPropertyChanged();
            }
        }

        int studyNumber;
        public int StudyNumber
        {
            get { return Utilities.StudyId; }
            set
            {
                studyNumber = value;
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
                IsPageUnavailableVisible = !value;
                OnPropertyChanged();
            }
        }


        int isItemEnabled;
        public int IsItemEnabled
        {
            get { return isItemEnabled; }
            set
            {
                isItemEnabled = value;
                OnPropertyChanged();
            }
        }

        bool isPageUnavailableVisible = false;
        public bool IsPageUnavailableVisible
        {
            get { return isPageUnavailableVisible; }
            set
            {
                isPageUnavailableVisible = value;
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

        public bool StudyInProcess
        {
            get => Get_Observations_By_StudyId().Count > 0;
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

        public List<Observation> Get_Observations_By_StudyId()
        {
            return ObservationRepo.GetItems()
                               .Where(x => x.StudyId == Utilities.StudyId).ToList();
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
            OperatorRepo.CreateTable();
            ObservationRepo.CreateTable();
            ActivityRepo.CreateTable();
            MergedActivityRepo.CreateTable();
            OperatorActivityRepo.CreateTable();
            SampleRepo.CreateTable();
        }
       

        public void CloseValidationView()
        {
            Opacity = 1;
            IsInvalid = false;
        }

        public void IsOkToNavigateAway()         {             var obs = ObservationRepo.GetItems().Where(x => x.StudyId == Utilities.StudyId                                                  && x.ObservationNumber == ObservationRound)                                                 .ToList();              if (obs.Count() > 0)             {
                if(Utilities.CurrentPageName != null && Utilities.CurrentPageName != "Current Study")
                {
                    Utilities.StudyPageInvalid = true;
                    Utilities.OnStudyPage = true;
                    Utilities.Navigate(new MainPageTabbedPage());
                }
            }
            else
            {
                Utilities.StudyPageInvalid = false;
            }

        }  
    }
}

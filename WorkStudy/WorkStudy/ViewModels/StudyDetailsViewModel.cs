using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class StudyDetailsViewModel: INotifyPropertyChanged
    {
        public Command SubmitDetails { get; set; }
        private readonly IBaseRepository<ActivitySampleStudy> sampleRepo;

        public StudyDetailsViewModel()
        {
            SubmitDetails = new Command(SubmitDetailsAndNavigate);
            sampleRepo = new BaseRepository<ActivitySampleStudy>(App.DatabasePath);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void SubmitDetailsAndNavigate()
        {
            ActivitySampleStudy sampleStudy = new ActivitySampleStudy 
            { 
                Name = Name ,
                Date = Date,
                IsRated = IsRated,
                StudiedBy  =StudiedBy,
                Department = Department,
                StudyNumber = StudyNumber
            };

            Utilities.StudyId = sampleRepo.SaveItem(sampleStudy);

            Navigate();
        }

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        string department;
        public string Department
        {
            get { return department; }
            set
            {
                department = value;
                OnPropertyChanged();
            }
        }

        string studiedBy;
        public string StudiedBy{
            get { return studiedBy; }
            set
            {
                studiedBy = value;
                OnPropertyChanged();
            }
        }

        DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }

        int studyNumber;
        public int StudyNumber
        {
            get { return studyNumber; }
            set
            {
                studyNumber = value;
                OnPropertyChanged();
            }
        }

        bool isRated;
        public bool IsRated{
            get { return isRated; }
            set
            {
                isRated = value;
                OnPropertyChanged();
            }
        }

        bool completed;
        public bool Completed{
            get { return completed; }
            set
            {
                completed = value;
                OnPropertyChanged();
            }
        }

        async void Navigate()
        {
            await Task.Delay(1000);
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddActivities());
        }

    }
}

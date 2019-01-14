using System;
using System.Linq;
using System.Windows.Input;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class StudyDetailsViewModel : BaseViewModel
    {
        public StudyDetailsViewModel(string conn) : base(conn) { ConstructorSetUp(); }

        public StudyDetailsViewModel() { ConstructorSetUp(); }

        public ICommand SubmitAndFocusOperators => new Command
        (
            (parameter) =>
            {
                ValidateValues();

                if (!IsInvalid)
                {
                    SampleStudy.IsRated = !IsUnRated;
                    Utilities.StudyId = SampleRepo.SaveItem(SampleStudy);
                    StudyNumber = Utilities.StudyId;
                    CreateUnratedActivities();

                    Utilities.RatedStudy = SampleStudy.IsRated;

                    var page = parameter as ContentPage;
                    var parentPage = page.Parent as TabbedPage;
                    parentPage.CurrentPage = parentPage.Children[2];

                    IsActive = false;
                }

                Opacity = 0.5;
                ShowClose = true;
            }
        );

        bool _isUnRated;
        public bool IsUnRated
        {
            get => _isUnRated;
            set
            {
                _isUnRated = value;
                OnPropertyChanged();
                Switch_Toggled();
            }
        }


        string studyType = "Rated";
        public string StudyType
        {
            get { return studyType; }
            set
            {
                studyType = value;
                OnPropertyChanged();
            }
        }

        void Switch_Toggled()
        {
            StudyType = _isUnRated == false ? "Rated" : "UnRated";
        }

        bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged();
            }
        }

        ActivitySampleStudy sampleStudy;
        public ActivitySampleStudy SampleStudy
        {
            get { return sampleStudy; }
            set
            {
                sampleStudy = value;
                OnPropertyChanged();
            }
        }

        private void ConstructorSetUp()
        {
            IsActive = true;

            Utilities.StudyId = 0;

            SampleStudy = new ActivitySampleStudy()
            {
                IsRated = !IsUnRated,
                Date = DateTime.Now,
                Time = DateTime.Now.TimeOfDay
            };

            IsPageVisible = true;

            int lastStudyId = 0;
            var studies = SampleRepo.GetItems()?.ToList();

            if (studies.Count > 0)
                lastStudyId = studies.OrderByDescending(x => x.Id)
                                        .FirstOrDefault().Id;

            lastStudyId = lastStudyId + 1;

            SampleStudy.StudyNumber = lastStudyId;
            CloseView = new Command(CloseValidationView);

        }

        private void ValidateValues()
        {
            ValidationText = "Please enter all study details";
            ShowClose = true;
            IsInvalid = true;
            Opacity = 0.2;

            if ((SampleStudy.Department != null && SampleStudy.Department?.Trim().Length > 0) &&
                (SampleStudy.Name != null && SampleStudy.Name?.Trim().Length > 0) &&
                (SampleStudy.StudiedBy != null && SampleStudy.StudiedBy?.Trim().Length > 0))
            {
                Opacity = 1;
                IsInvalid = false;
            }

        }

        public void CreateUnratedActivities()
        {
            var activityName = ActivityNameRepo.GetItems().FirstOrDefault(x => x.Name == "ABSENT");

            if(activityName == null)
                activityName = new ActivityName { Name = "ABSENT" };
                
            var unrated1 = new Activity()
            {
                ActivityName = activityName,
                IsEnabled = true,
                Rated = false,
                StudyId = Utilities.StudyId,
                DeleteIcon = string.Empty
            };

            activityName = ActivityNameRepo.GetItems().FirstOrDefault(x => x.Name == "INACTIVE");

            if (activityName == null)
                activityName = new ActivityName { Name = "INACTIVE" };
                
            var unrated2 = new Activity()
            {
                ActivityName = activityName,
                IsEnabled = true,
                Rated = false,
                StudyId = Utilities.StudyId,
                DeleteIcon = string.Empty
            };

            activityName = ActivityNameRepo.GetItems().FirstOrDefault(x => x.Name == "OTHER");

            if (activityName == null)
                activityName = new ActivityName { Name = "OTHER" };

            var unrated3 = new Activity()
            {
                ActivityName = activityName,
                IsEnabled = true,
                Rated = false,
                StudyId = Utilities.StudyId,
                DeleteIcon = string.Empty
            };

            ActivityNameRepo.SaveItem(unrated1.ActivityName);
            ActivityRepo.SaveItem(unrated1);
            ActivityRepo.UpdateWithChildren(unrated1);

            ActivityNameRepo.SaveItem(unrated2.ActivityName);
            ActivityRepo.SaveItem(unrated2);
            ActivityRepo.UpdateWithChildren(unrated2);

            ActivityNameRepo.SaveItem(unrated3.ActivityName);
            ActivityRepo.SaveItem(unrated3);
            ActivityRepo.UpdateWithChildren(unrated3);

        }
    }
}
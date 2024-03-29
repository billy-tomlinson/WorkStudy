﻿using System;
using System.Linq;
using System.Windows.Input;
using WorkStudy.Model;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class StudyDetailsViewModel : BaseAlarmViewModel
    {
        public StudyDetailsViewModel(string conn) : base(conn) { ConstructorSetUp(); }

        public StudyDetailsViewModel()
        {
            ConstructorSetUp(); 
        }

        public ICommand SubmitAndFocusOperators => new Command
        (
            (parameter) =>
            {
                ValidateValues();

                if (!IsInvalid)
                {
                    StudyPageOpacity = 0.5;
                    SampleStudy.IsRated = !IsUnRated;
                    Utilities.StudyId = SampleRepo.SaveItem(SampleStudy);

                    SampleRepo.ExecuteSQLCommand("UPDATE ACTIVITYSAMPLESTUDY SET STUDYNUMBER = " + Utilities.StudyId + " WHERE ID == " + Utilities.StudyId);

                    AlarmRepo.SaveItem(new AlarmDetails
                    {
                        IsActive = IsAlarmEnabled,
                        Type = AlarmType,
                        Interval = IntervalTime,
                        StudyId = Utilities.StudyId
                    }
                    );

                    StudyNumber = Utilities.StudyId;
                    CreateUnratedActivities();

                    Utilities.RatedStudy = SampleStudy.IsRated;

                    IsActive = false;
                    Opacity = 1.0;
                    IsPageEnabled = true;

                    Utilities.Navigate(new MainPageTabbedPage(true));
                }
                else
                    Opacity = 0.2;
                    IsPageEnabled = false;

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

        double studyPageOpacity = 1;
        public double StudyPageOpacity
        {
            get { return studyPageOpacity; }
            set
            {
                studyPageOpacity = value;
                OnPropertyChanged();
            }
        }

        string studyType = "RATED";
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
            StudyType = _isUnRated == false ? "RATED" : "UNRATED";
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

            AlarmType = Interval;
            IsRandom = false;
            IsAlarmEnabled = false;
            IntervalMinutes = string.Empty;

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
            else
            {
                var study = new ActivitySampleStudy { Name = "Initial Study to Invoke ID count" };
                lastStudyId = SampleRepo.SaveItem(study);
                SampleRepo.DeleteItem(study);
            }

            lastStudyId = lastStudyId + 1;

            SampleStudy.StudyNumber = lastStudyId;
            CloseView = new Command(CloseValidationView);
            IsPageEnabled = true;

        }

        private void ValidateValues()
        {
            ValidationText = "Please enter all study details";
            ShowClose = true;
            IsInvalid = true;
            IsPageEnabled = false;
            Opacity = 0.2;

            var success = int.TryParse(IntervalMinutes, out int result);

            if (result < 1) success = false;

            if (!AlarmIntervalIsValid(success)) return;

            IntervalTime = result * 60;

            if ((SampleStudy.Department != null && SampleStudy.Department?.Trim().Length > 0) &&
                (SampleStudy.Name != null && SampleStudy.Name?.Trim().Length > 0) &&
                (SampleStudy.StudiedBy != null && SampleStudy.StudiedBy?.Trim().Length > 0))
            {
                Opacity = 1;
                IsInvalid = false;
                IsPageEnabled = true;
            }

        }

        public bool AlarmIntervalIsValid(bool success)
        {

            if (!success)
            {
                ValidationText = "Please enter interval in minutes between 1 and 99";
                Opacity = 0.2;
                IsInvalid = true;
                IsPageEnabled = false;
                IsAlarmEnabled = false;
                ShowClose = true;
                Switch_Toggled_Enabled();
                return false;

            }
            {
                IsPageEnabled = true;
                return true;
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
                DeleteIcon = string.Empty,
                ItemColour = Utilities.InactiveColour,
                ObservedColour = Utilities.InactiveColour

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
                DeleteIcon = string.Empty,
                ItemColour = Utilities.InactiveColour,
                ObservedColour = Utilities.InactiveColour
            };

            //activityName = ActivityNameRepo.GetItems().FirstOrDefault(x => x.Name == "OTHER");

            //if (activityName == null)
            //    activityName = new ActivityName { Name = "OTHER" };

            //var unrated3 = new Activity()
            //{
            //    ActivityName = activityName,
            //    IsEnabled = true,
            //    Rated = false,
            //    StudyId = Utilities.StudyId,
            //    DeleteIcon = string.Empty,
            //    ItemColour = Utilities.InactiveColour,
            //    ObservedColour = Utilities.InactiveColour
            //};

            ActivityNameRepo.SaveItem(unrated1.ActivityName);
            ActivityRepo.SaveItem(unrated1);
            ActivityRepo.UpdateWithChildren(unrated1);

            ActivityNameRepo.SaveItem(unrated2.ActivityName);
            ActivityRepo.SaveItem(unrated2);
            ActivityRepo.UpdateWithChildren(unrated2);

            //ActivityNameRepo.SaveItem(unrated3.ActivityName);
            //ActivityRepo.SaveItem(unrated3);
            //ActivityRepo.UpdateWithChildren(unrated3);

        }
    }
}
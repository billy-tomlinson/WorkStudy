using System.Collections.ObjectModel;
using System.Linq;
using TimeStudy.Model;
using TimeStudy.Pages;
using TimeStudy.Services;
using Xamarin.Forms;
using TimeStudy.Custom;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using TimeStudyApp.Model;

namespace TimeStudy.ViewModels
{
    public class ExistingStudiesViewModel : BaseViewModel
    {
        bool completed;
        public Command DeleteSelected { get; set; }

        public ExistingStudiesViewModel(bool completed)
        {
            this.completed = completed;
            DeleteSelected = new Command(DeleteSelectedEvent);
            Override = new Command(OverrideEvent);
            ConfirmationOverride = new Command(ConfirmationOverrideEvent);

            ActivitySamples = new ObservableCollection<Model.RatedTimeStudy>(RatedTimeStudyRepo.GetAllWithChildren());
            RefreshActivitySamples(completed);
        }

        private void RefreshActivitySamples(bool completed)
        {
            if (completed)
            {
                var historic = StudyHistoryVersionRepo.GetItems();
                var historicStudies = new List<Model.RatedTimeStudy>();
                foreach (var item in historic)
                {
                    var study = RatedTimeStudyRepo.GetItem(item.StudyId);
                    if (study != null)
                    {
                        study.Version = item.Id;
                        study.Date = item.Date;
                        study.Time = item.Time;
                        historicStudies.Add(study);
                    }
                }

                ActivitySamples = new ObservableCollection<Model.RatedTimeStudy>(historicStudies);
            }
        }

        static ObservableCollection<Model.RatedTimeStudy> activitySamples;
        public ObservableCollection<Model.RatedTimeStudy> ActivitySamples
        {
            get => activitySamples;
            set
            {
                activitySamples = value;
                OnPropertyChanged();
            }
        }

        public Command ItemClickedCommand
        {
            get { return Navigate(); }
        }

        void ConfirmationOverrideEvent(object sender)
        {
            var value = Utilities.VersionDetails;

            if (ConfirmationStudyNumber == value.StudyId && RandomGeneratedCode == ConfirmationValidationCode)
            {
                IsInvalid = true;
                IsConfirmation = false;
                Opacity = 0.2;
                IsPageEnabled = false;
                ValidationText = "To confirm you want to delete study " + ConfirmationStudyNumber + " , version " + value.Version + " click OK.";
                Utilities.DeleteConfirmedDisplayed = true;
            }
            else
            {
                ValidationText = "You have entered incorrect details.";
                IsOverrideVisible = false;
                ShowClose = true;
                ShowOkCancel = false;
                IsPageUnavailableVisible = false;
                Opacity = 0.2;
                CloseColumnSpan = 2;
                IsInvalid = true;
                IsConfirmation = false;
                IsPageEnabled = false;
                Utilities.StudyId = value.StudyId;
                return;
            }

            ConfirmationValidationCode = null;
            ConfirmationStudyNumber = null;
        }

        private void DeleteAllRecords(RatedTimeStudy.VersionDetails value)
        {
            if (value.Version > 0)// this is a completed study
            {
                var sample = StudyHistoryVersionRepo.GetItem(value.Version);

                if (sample.StudyId == value.StudyId)
                {
                    StudyHistoryVersionRepo.ExecuteSQLCommand("DELETE FROM RATEDTIMESTUDYHISTORYVERSION WHERE STUDYID = " + sample.StudyId + " AND ID = " + sample.Id);
                    RefreshActivitySamples(true);
                }
            }
            else if (value.Version == 0)// this is a blueprint study  with no attached completed studies
            {
                RatedTimeStudyRepo.ExecuteSQLCommand("DELETE FROM RATEDTIMESTUDY WHERE ID = " + value.StudyId);
                WorkElementRepo.ExecuteSQLCommand("DELETE FROM WORKELEMENT WHERE STUDYID = " + value.StudyId);
                WorkElementNameRepo.ExecuteSQLCommand("DELETE FROM WORKELEMENTNAME WHERE ID NOT IN (SELECT ACTIVITYNAMEID FROM WORKELEMENT)");
                RefreshActivitySamples(true);
            }

            Utilities.StudyId = 0;
            Utilities.VersionDetails = null;

            IsInvalid = false;
            IsConfirmation = false;
            Opacity = 1;
            IsPageEnabled = true;

        }

        void OverrideEvent(object sender)
        {
            if(Utilities.DeleteConfirmedDisplayed)
            {
                DeleteAllRecords(Utilities.VersionDetails);
                Utilities.DeleteConfirmedDisplayed = false;
            }
            else
            {
                var rand = new Random();
                int num = rand.Next(1000, 9999);

                Utilities.RandomGeneratedCode = num;
                RandomGeneratedCode = Utilities.RandomGeneratedCode;

                ConfirmationStudyNumberLabel = "Study Number : " + Utilities.StudyId;
                ConfirmationValidationCodeLabel = "Code : " + Utilities.RandomGeneratedCode;

                var value = Utilities.StudyId;

                ValidationText = "You are about to delete study " + value + ". Enter Study number and Code and press OK to confirm.";
                IsOverrideVisible = false;
                ShowClose = true;
                ShowOkCancel = true;
                IsPageUnavailableVisible = false;
                Opacity = 0.2;
                CloseColumnSpan = 1;
                IsInvalid = false;
                IsConfirmation = true;
                IsPageEnabled = false;
                Utilities.StudyId = value;
                return;
            }
        }

        void DeleteSelectedEvent(object sender)
        {
            var value = (RatedTimeStudy.VersionDetails)sender;

            Utilities.VersionDetails = value;

            if (value.Version == 0) //this means its the blueprint
            {
                var studyVersionCount = StudyHistoryVersionRepo.ExecuteScalarSQLCommand<int>("SELECT COUNT(*) FROM RATEDTIMESTUDYHISTORYVERSION WHERE STUDYID = " + value.StudyId);

                if (studyVersionCount > 0)
                {
                    ValidationText = "Cannot delete study " + value.StudyId + ", version " + value.Version + " until all versions of this study have been deleted in the Completed Studies tab.";
                    IsOverrideVisible = false;
                    ShowClose = true;
                    ShowOkCancel = false;
                    CloseColumnSpan = 2;
                }
                else
                {
                    ValidationText = "Are you sure you want to delete study " + value.StudyId + ", version " + value.Version +  " ? This cannot be undone and all data related to this study will be deleted";
                    IsOverrideVisible = false;
                    ShowOkCancel = true;
                    CloseColumnSpan = 1;
                }
            }
            else if (value.Version > 0) //this means its a completed study
            {
                ValidationText = "Are you sure you want to delete study " + value.StudyId + " , version " + value.Version + " ? This cannot be undone and all data related to this study will be deleted";
                IsOverrideVisible = false;
                ShowOkCancel = true;
                CloseColumnSpan = 1;
            }

            IsOverrideVisible = false;
            IsPageUnavailableVisible = false;
            Opacity = 0.2;
            IsInvalid = true;
            IsPageEnabled = false;
            IsConfirmation = false;
            Utilities.StudyId = value.StudyId;
            return;

        }

        public Command Navigate()
        {

            return new  Command(async (item) =>
            {
                IsBusy = true;
                Task navigateTask = Task.Run( async () =>
                {
                    await Task.Delay(200);
                    Opacity = 0.2;

                    var study = item as Model.RatedTimeStudy;
                    study.ObservedColour = Xamarin.Forms.Color.Silver.GetShortHexString();
                    Utilities.StudyId = study.Id;
                    Utilities.StudyVersion = study.Version;
                    Utilities.RatedStudy = study.IsRated;
                    Utilities.IsCompleted = completed;

                    Device.BeginInvokeOnMainThread(async  () =>
                    {
                        if (!completed)
                           await Utilities.Navigate(new TimeStudyMainPageTabbedPage());
                        else
                            await Utilities.Navigate(new TimeStudyReportsPage());
                    });
                });

                await navigateTask;

            });
        }
    }
}

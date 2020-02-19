using System.Collections.ObjectModel;
using System.Linq;
using WorkStudy.Model;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;
using WorkStudy.Custom;
using System.Threading.Tasks;

namespace WorkStudy.ViewModels
{
    public class ExistingStudiesViewModel : BaseViewModel
    {
        bool completed;
        public Command DeleteSelected { get; set; }

        public ExistingStudiesViewModel(bool completed)
        {
            DeleteSelected = new Command(DeleteSelectedEvent);
            Override = new Command(OverrideEvent);

            Opacity = 1;
            IsBusy = false;

            this.completed = completed;
            ActivitySamples = new ObservableCollection<ActivitySampleStudy>(SampleRepo.GetItems()
                                  .Where(_ => _.Completed == completed));
        }


        void OverrideEvent(object sender)
        {
            IsInvalid = false;
            Opacity = 1;
            IsPageEnabled = true;

            var value = Utilities.StudyId;

            var sample = SampleRepo.GetItem(value);

            SampleRepo.ExecuteSQLCommand("DELETE FROM ACTIVITYSAMPLESTUDY WHERE ID = " + value);
            AlarmRepo.ExecuteSQLCommand("DELETE FROM ALARMDETAILS WHERE STUDYID = " + value);
            ObservationRepo.ExecuteSQLCommand("DELETE FROM OBSERVATION WHERE STUDYID = " + value);
            ObservationHistoricRepo.ExecuteSQLCommand("DELETE FROM OBSERVATIONHISTORIC WHERE STUDYID = " + value);
            ObservationRoundStatusRepo.ExecuteSQLCommand("DELETE FROM OBSERVATIONROUNDSTATUS WHERE STUDYID = " + value);
            OperatorRepo.ExecuteSQLCommand("DELETE FROM OPERATOR WHERE STUDYID = " + value);
            MergedActivityRepo.ExecuteSQLCommand("DELETE FROM MERGEDACTIVITIES WHERE ACTIVITYID IN (SELECT ID FROM ACTIVITY WHERE STUDYID == " + value + ")");
            ActivityNameRepo.ExecuteSQLCommand("DELETE FROM ACTIVITYNAME WHERE ID IN (SELECT ACTIVITYNAMEID FROM ACTIVITY WHERE STUDYID == " + value + ")");
            ActivityRepo.ExecuteSQLCommand("DELETE FROM ACTIVITY WHERE STUDYID = " + value);
            ActivitySamples = new ObservableCollection<ActivitySampleStudy>(SampleRepo.GetItems()
                                  .Where(_ => _.Completed == completed));
        }

        void DeleteSelectedEvent(object sender)
        {
            var value = (int)sender;

            ValidationText = "Are you sure you want to delete this study. This cannot be undone and all data related to this study will be deleted?";
            IsOverrideVisible = true;
            ShowClose = true;
            ShowOkCancel = false;
            IsPageUnavailableVisible = false;
            Opacity = 0.2;
            CloseColumnSpan = 1;
            IsInvalid = true;
            IsPageEnabled = false;
            Utilities.StudyId = value;
            return;
        }

        static ObservableCollection<ActivitySampleStudy> activitySamples;
        public ObservableCollection<ActivitySampleStudy> ActivitySamples
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

        public Command Navigate()
        {

            return new  Command(async (item) =>
            {
                IsBusy = true;
                Task navigateTask = Task.Run( async () =>
                {
                    await Task.Delay(200);
                    Opacity = 0.2;

                    var study = item as ActivitySampleStudy;
                    study.ObservedColour = Xamarin.Forms.Color.Silver.GetShortHexString();
                    Utilities.StudyId = study.Id;
                    Utilities.RatedStudy = study.IsRated;
                    Utilities.IsCompleted = completed;

                    Device.BeginInvokeOnMainThread(async  () =>
                    {
                        if (!completed)
                           await Utilities.Navigate(new MainPageTabbedPage());
                        else
                            await Utilities.Navigate(new ReportsPage());
                    });
                });

                await navigateTask;

            });
        }
    }
}

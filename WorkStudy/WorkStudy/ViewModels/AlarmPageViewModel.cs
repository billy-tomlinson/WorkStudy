using WorkStudy.Services;
using WorkStudy.Model;
using System.Linq;

namespace WorkStudy.ViewModels
{
    public class AlarmPageViewModel : BaseAlarmViewModel
    {

        public bool CancelAlarm { get; set; }

        public ActivitySampleStudy ActivtySample { get; set; }

        public string StudyType { get; set; }

        public bool ContinueTimer { get; set; } = true;


        public AlarmPageViewModel()
        {
            if (!IsPageVisible) return;

            PageLoading = true;
            ActivtySample = SampleRepo.GetItem(Utilities.StudyId);

            StudyType = ActivtySample.IsRated == false ? "RATED" : "UNRATED";

            AlarmDetails = AlarmRepo.GetItems()
                .SingleOrDefault(x => x.StudyId == Utilities.StudyId) ?? new AlarmDetails();

            AlarmType = AlarmDetails.Type != string.Empty ? AlarmDetails.Type : Interval;
            IsRandom = AlarmDetails.Type != Interval;
            IsAlarmEnabled = AlarmDetails.IsActive;
            IntervalMinutes = (AlarmDetails.Interval / 60).ToString();
            PageLoading = false;
        }

    }
}

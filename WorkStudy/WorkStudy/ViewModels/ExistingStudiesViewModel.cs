using System.Collections.ObjectModel;
using System.Linq;
using WorkStudy.Model;
using WorkStudy.Pages;
using WorkStudy.Services;
using Xamarin.Forms;
using WorkStudy.Custom;

namespace WorkStudy.ViewModels
{
    public class ExistingStudiesViewModel : BaseViewModel
    {
        bool completed;
        public ExistingStudiesViewModel(bool completed)
        {
            Opacity = 1;
            IsBusy = false;

            this.completed = completed;
            ActivitySamples = new ObservableCollection<ActivitySampleStudy>(SampleRepo.GetItems()
                                  .Where(_ => _.Completed == completed));
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
            return new Command(async (item) =>
            {
                Opacity = 0.2;
                IsBusy = true;

                var study = item as ActivitySampleStudy;
                //study.ObservedColour = Xamarin.Forms.Color.Silver.GetShortHexString();
                Utilities.StudyId = study.Id;
                Utilities.RatedStudy = study.IsRated;
                Utilities.IsCompleted = completed;

                if(!completed)
                   await Utilities.Navigate(new MainPageTabbedPage());
                else
                    await Utilities.Navigate(new ReportsPage());

                Opacity = 1;
                IsBusy = false;
            });
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Linq;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class AllActivitiesViewModel : BaseViewModel
    {
        public Command Submit { get; set; }

        public AllActivitiesViewModel()
        {
            Submit = new Command(ActivitySelectedEvent);

            Utilities.StudyId = 1;
            IsPageVisible = true;
            ItemsCollection = Get_All_ActivityNames();
        }

        static ObservableCollection<ActivityName> itemsCollection;
        public ObservableCollection<ActivityName> ItemsCollection
        {
            get => itemsCollection;
            set
            {
                itemsCollection = value;
                OnPropertyChanged();
            }
        }

        void ActivitySelectedEvent(object sender)
        {

        }
    }
}
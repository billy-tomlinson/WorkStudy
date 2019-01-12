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
            ItemsCollection = Get_Previous_Enabled_Activities();
            foreach (var item in ItemsCollection)
            {
                item.DeleteIcon = string.Empty;
                item.SettingsIcon = string.Empty;
            }
        }

        static ObservableCollection<Activity> itemsCollection;
        public ObservableCollection<Activity> ItemsCollection
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

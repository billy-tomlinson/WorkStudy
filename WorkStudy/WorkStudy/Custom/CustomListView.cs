using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WorkStudy.Model;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.Custom
{
    public class CustomListView : ListView
    {

        public static BindableProperty ItemClickedCommandProperty =
            BindableProperty.Create(nameof(ItemClickedCommand), typeof(ICommand), typeof(CustomListView), null);

        public CustomListView(ListViewCachingStrategy strategy) : base(strategy)
        {
            this.ItemTapped += OnItemTapped;

            this.ItemDisappearing += OnDisappearing;
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                ItemClickedCommand?.Execute(e.Item);
                SelectedItem = null;
            }
        }


        private void OnDisappearing(object sender, EventArgs e)
        {
            CustomListView view = (CustomListView)sender;
            var itemSource = view.ItemsSource.GetType();

            if (itemSource == typeof(ObservableCollection<OperatorObservation>) && Utilities.SetUpForNextObservationRound == true)
            {
                var obs = (ObservableCollection<OperatorObservation>)view.ItemsSource;
                var observations = obs.OrderBy(x => x.Id).FirstOrDefault();
                this.ScrollTo(observations, ScrollToPosition.End, true);
                Utilities.SetUpForNextObservationRound = false;
            }

            if (itemSource == typeof(ObservableCollection<OperatorRunningTotal>) && Utilities.CloseRunningTotals == true)
            {
                var obs = (ObservableCollection<OperatorRunningTotal>)view.ItemsSource;
                var observations = obs.OrderBy(x => x.ActivityId).FirstOrDefault();
                this.ScrollTo(observations, ScrollToPosition.End, true);
                Utilities.CloseRunningTotals = false;
            }
        }

        public ICommand ItemClickedCommand
        {
            get
            {
                return (ICommand)this.GetValue(ItemClickedCommandProperty);
            }
            set
            {
                this.SetValue(ItemClickedCommandProperty, value);
            }
        }

    }
}

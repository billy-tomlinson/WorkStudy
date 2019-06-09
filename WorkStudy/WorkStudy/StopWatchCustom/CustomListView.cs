using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WorkStudy.Services;
using Xamarin.Forms;

namespace StopWatch
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

        private void OnDisappearing(object sender, EventArgs e)
        {
            CustomListView view = (CustomListView)sender;

            if (Utilities.LapButtonClicked == true)
            {
                var obs = (ObservableCollection<LapTime>)view.ItemsSource;
                var observations = obs.OrderByDescending(x => x.Count).FirstOrDefault();
                this.ScrollTo(observations, ScrollToPosition.End, true);

                Utilities.LapButtonClicked = false;
            }
        }
    }
}

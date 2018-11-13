
using System.Collections.Generic;
using SQLite;
using Xamarin.Forms;
using WorkStudy.Model;

namespace WorkStudy
{
    public partial class ReportsPage : ContentPage
    {
        public ReportsPage()
        {
            InitializeComponent();
            chart.Source = ImageSource.FromFile("chart.png");
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void Submit_Clicked(object sender, System.EventArgs e)
        {
            DataBase();
            chartView.IsVisible = true;
        }

        void Ok_Clicked(object sender, System.EventArgs e)
        {
            DataBase();
            chartView.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        void DataBase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {

                var sample = new ActivitySampleStudy();
                sample.Name = "Billy";

                conn.CreateTable<ActivitySampleStudy>();
                int itemsInserted = conn.Insert(sample);

            }

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                var sample = new ActivitySampleStudy();
                conn.CreateTable<ActivitySampleStudy>();
                List<ActivitySampleStudy> notes = conn.Table<ActivitySampleStudy>().ToList();
                DisplayAlert(notes.Count.ToString(), "OK", "ok");
            }
        }
    }


}

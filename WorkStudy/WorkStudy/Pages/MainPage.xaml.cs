using WorkStudy.Model;
using Xamarin.Forms;
using WorkStudy.ViewModels;
using System;
using WorkStudy.Custom;

namespace WorkStudy
{
    public partial class MainPage : ContentPage
    {
        static Operator _operator;
        static string Name;
        MainPageViewModel viewModel;

        void Submit_Clicked(object sender, System.EventArgs e)
        {
            
            var vm = BindingContext as MainPageViewModel;
            vm?.UpdateStudyNumber();
            Navigate();
        }

        //public void CreateActivitiesGrid()
        //{
        //    var items = viewModel.Activities;
        //    double count = (double)items.Count / 3;
        //    var rounded = Math.Ceiling(count);

        //    for (int i = 0; i < rounded; i++)
        //    {
        //        gridLayout.RowDefinitions.Add(new RowDefinition());
        //    }

        //    gridLayout.ColumnDefinitions.Add(new ColumnDefinition());
        //    gridLayout.ColumnDefinitions.Add(new ColumnDefinition());
        //    gridLayout.ColumnDefinitions.Add(new ColumnDefinition());

        //    var productIndex = 0;

        //    for (int rowIndex = 0; rowIndex < rounded; rowIndex++)
        //    {
        //        for (int columnIndex = 0; columnIndex < 3; columnIndex++)
        //        {
        //            if (productIndex >= items.Count)
        //            {
        //                break;
        //            }

        //            var product = items[productIndex];
        //            productIndex += 1;

        //            var label = new CustomButton
        //            {
        //                Text = product.Name,
        //                ActivityId = product.Id,
        //                Command = viewModel.ActivitySelected,
        //                VerticalOptions = LayoutOptions.Center,
        //                HorizontalOptions = LayoutOptions.Center,
        //                CommandParameter = product.Id,
        //                BackgroundColor = Color.Ivory,
        //                MinimumWidthRequest = 150,
        //                MinimumHeightRequest = 40,
        //                FontSize = 12,
        //                WidthRequest = 150,
        //                HeightRequest = 40

        //            };

        //            gridLayout.Children.Add(label, columnIndex, rowIndex);
        //        }
        //    }
        //}
       
        void Rating_Clicked(object sender, System.EventArgs e)
        {
            var vm = BindingContext as MainPageViewModel;
            vm?.ShowOrHideOperators(_operator);
            ratingView.IsVisible = false;
        }

        void End_Clicked(object sender, System.EventArgs e)
        {

            NavigateToReports();
        }

        public MainPage()
        {
            InitializeComponent();
            viewModel = BindingContext as MainPageViewModel;
            //CreateActivitiesGrid();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void ListViewItem_Tabbed(object sender, ItemTappedEventArgs e)
        {
            _operator = e.Item as Operator;
            Name = _operator.Name;
            var vm = BindingContext as MainPageViewModel;
            //displayNameEntry.Text = Name;
            activityView.IsVisible = true;
            vm?.ShowOrHideOperators(_operator);
        }

        async void Navigate()
        {
            await Navigation.PushModalAsync(new MainPage());
        }

        async void NavigateToReports()
        {
            await System.Threading.Tasks.Task.Delay(1000);
            await Navigation.PushModalAsync(new ReportsPage());
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}

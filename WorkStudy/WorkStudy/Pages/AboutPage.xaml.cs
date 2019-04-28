using System;
using System.Collections.Generic;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            timeStudy.Source = ImageSource.FromFile("stopwatch.png");
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetBackButtonTitle(this, "");
        }

        protected override void OnAppearing()
        {
            Utilities.ClearNavigation();
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}

using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class EditActivitiesPage : ContentPage
    {
        public EditActivitiesPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}

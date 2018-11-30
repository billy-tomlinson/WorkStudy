using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class EditActivities : ContentPage
    {
        public EditActivities()
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

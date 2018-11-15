using System;
using System.Collections;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WorkStudy
{
    public partial class PopUpDemo : ContentPage
    {
        public PopUpDemo()
        {
            InitializeComponent();
            IEnumerable items = new List<string> { "Xamarin.Forms", "Xamarin.iOS", "Xamarin.Android", "Xamarin Monkeys" };
            sampleList.ItemsSource = items;
        }

        private void btnPopupButton_Clicked(object sender, EventArgs e)
        {
            popupLoginView.IsVisible = true;
            activityIndicator.IsRunning = true;
        }

        private void btnLoginButton_Clicked(object sender, EventArgs e)
        {
            popupLoginView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }
    }
}

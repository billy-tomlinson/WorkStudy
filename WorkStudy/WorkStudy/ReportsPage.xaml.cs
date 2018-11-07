using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WorkStudy
{
    public partial class ReportsPage : ContentPage
    {
        public ReportsPage()
        {
            InitializeComponent();
        }

        void Submit_Clicked(object sender, System.EventArgs e)
        {
            DisplayAlert("Alert", "Some Riveting Report or Other", "OK");
        }
    }


}

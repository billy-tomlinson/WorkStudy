using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WorkStudy
{
    public partial class StudyStartPAge : ContentPage
    {
        public StudyStartPAge()
        {
            InitializeComponent();
        }

        void Submit_Clicked(object sender, System.EventArgs e)
        {
            Navigate();
        }

        async void Navigate()
        {
            await System.Threading.Tasks.Task.Delay(1000);
            await Navigation.PushAsync(new MainPage());
        }
    }
}

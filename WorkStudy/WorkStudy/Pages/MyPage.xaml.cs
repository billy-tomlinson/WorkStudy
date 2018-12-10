using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WorkStudy
{
    public partial class MyPage : TabbedPage
    {
        public MyPage()
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

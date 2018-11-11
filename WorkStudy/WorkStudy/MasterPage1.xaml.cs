using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WorkStudy
{
    public partial class MasterPage1 : ContentPage
    {
        public MasterPage1()
        {
            InitializeComponent();
            BindingContext = new MasterViewModel();
        }
    }
}

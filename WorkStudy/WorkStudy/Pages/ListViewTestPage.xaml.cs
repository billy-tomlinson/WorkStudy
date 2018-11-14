using System;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class ListViewTestPage : ContentPage
    {
        
        public ListViewTestPage()
        {
            BindingContext = new CommentsPageViewModel();
            InitializeComponent();
        }
       
    }
}

using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class CompletedStudiesPage : ContentPage
    {
        public CompletedStudiesPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            listOfStudies.BindingContext = new ExistingStudiesViewModel(true);
        }

        //public CompletedStudiesPage(bool completed)
        //{
        //    InitializeComponent();
        //    NavigationPage.SetHasNavigationBar(this, false);
        //    listOfStudies.BindingContext = new ExistingStudiesViewModel(completed);
        //}
    }
}

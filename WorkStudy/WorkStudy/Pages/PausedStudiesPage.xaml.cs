using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class PausedStudiesPage : ContentPage
    {
        public PausedStudiesPage()
        {
        }

        public PausedStudiesPage(bool completed)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            listOfStudies.BindingContext = new ExistingStudiesViewModel(completed);

        }
    }
}

using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class CompletedStudiesPage : ContentPage
    {
        public CompletedStudiesPage(bool completed)
        {
            InitializeComponent();
            listOfStudies.BindingContext = new ExistingStudiesViewModel(completed);
        }
    }
}

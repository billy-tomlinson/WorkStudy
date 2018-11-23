using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class PausedStudiesPage : ContentPage
    {
        public PausedStudiesPage(bool completed)
        {
            InitializeComponent();
            listOfStudies.BindingContext = new ExistingStudiesViewModel(completed);

        }
    }
}

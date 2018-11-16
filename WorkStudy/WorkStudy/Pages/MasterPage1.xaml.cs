
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkStudy
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage1 : ContentPage
    {
        public MasterPage1()
        {
            InitializeComponent();
            BindingContext = new MasterViewModel();
        }
    }
}

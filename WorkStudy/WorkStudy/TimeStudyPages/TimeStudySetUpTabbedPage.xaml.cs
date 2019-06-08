
using TimeStudy.Services;
using Xamarin.Forms;

namespace TimeStudy.Pages
{
    public partial class TimeStudySetUpTabbedPage : TabbedPage
    {
        public TimeStudySetUpTabbedPage()
        {
            this.BackgroundColor = Color.FromHex(Utilities.TimeStudyBackGroundColour);
            this.BarBackgroundColor = Color.FromHex(Utilities.TimeStudyTabBarBackgroundColour);
            InitializeComponent();
        }
    }
}

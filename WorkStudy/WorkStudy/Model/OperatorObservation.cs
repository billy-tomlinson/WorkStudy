using SQLite;
using WorkStudy.Services;

namespace WorkStudy.Model
{
    public class OperatorObservation
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ActivityName { get; set; }

        public int? Rating { get; set; }

        public bool IsRated { get; set; }

        public System.Drawing.Color ObservedColour { get; set; }

        public bool LimitsOfAccuracy { get; set; }

        public string TotalPercentagePerOperator { get; set; }

        public double TotalPercentageDouble { get; set; }

        [Ignore]
        public bool OperatorPercentageIsVisible { get; set; }

        public string SettingsIcon { get; set; } = Utilities.CommentsImage;

        public string Comment { get; set; }

        public bool CommentsVisible { get; set; }
    }
}

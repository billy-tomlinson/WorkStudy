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

        public double TotalPercentage { get; set; }
    }
}

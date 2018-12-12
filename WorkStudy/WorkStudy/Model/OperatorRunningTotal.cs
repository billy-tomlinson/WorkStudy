namespace WorkStudy.Model
{
    public class OperatorRunningTotal
    {
        public int OperatorId { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int NumberOfObservations { get; set; }
        public double Percentage { get; set; }
        public string PercentageFormatted { get; set; }
    }
}

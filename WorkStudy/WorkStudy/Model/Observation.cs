using System;

namespace WorkStudy.Model
{
    public class Observation : BaseEntity
    {
        public int OperatorId { get; set; }
        public int Activity { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
    }
}

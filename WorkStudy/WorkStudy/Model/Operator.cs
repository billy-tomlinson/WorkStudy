using System.Collections.Generic;

namespace WorkStudy.Model
{
    public class Operator
    {
        public int Id { get; set; }
        public int StudyId { get; set; }
        public string Name { get; set; }
        public List<Activity> Activities { get; set; }

    }
}

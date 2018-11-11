using System.Collections.Generic;

namespace WorkStudy
{
    public interface IActivitySampleStudyRepository
    {
        List<ActivitySampleStudy> GetAllActivitySampleStudies();  
   
        ActivitySampleStudy GetActivitySampleStudy(int id);  
   
        void DeleteAllStudies();  
  
        void DeleteStudy(int id);  
   
        void InsertStudy(ActivitySampleStudy study);  
   
        void UpdateStudy(ActivitySampleStudy study);  
    }
}

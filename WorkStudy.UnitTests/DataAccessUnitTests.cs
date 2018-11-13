using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkStudy.Model;
using WorkStudy.Services;

namespace WorkStudy.UnitTests
{
    [TestClass]
    public class DataAccessUnitTests
    {
        [TestClass]
        public class DataAccessTests
        {
            private const string connString = "WorkStudy.db3";
            DataAccessService service;
            public DataAccessTests()
            {
                service = new DataAccessService(connString);
            }

            [TestMethod]
            public void AddAndRetrieveActivitySample()
            {
                var activityStudy = new ActivitySampleStudy()
                {
                    Name = "Activity One",
                    Date = DateTime.Now,
                    StudyNumber = 1,
                    IsRated = true,
                    StudiedBy = "Billy",
                    Department = "PaintShop"
                };

                var id = service.Add(activityStudy);

                var sample = service.GetActivitySampleStudyById(id);

                Assert.AreEqual(id, sample.Id);
                Assert.AreEqual(activityStudy.Department, sample.Department);
            }

            [TestMethod]
            public void GetActivitySamples()
            {
                var studies = service.GetAll<ActivitySampleStudy>();
                Assert.IsTrue(studies.Count > 0); 
            }

            [TestMethod]
            public void AddAndRetrieveActivity()
            {
                var activity = new Activity()
                {
                    Name = "Activity One",
                    Comment = "Some comment or other",
                    Date = DateTime.Now,
                    IsEnabled = true
                };

                var id = service.Add(activity);

                var value = service.GetActivityById(id);
                Assert.AreEqual(id, value.Id);
                Assert.AreEqual(value.Name, activity.Name);
            }

            [TestMethod]
            public void GetActivities()
            {
                var activities = service.GetAll<Activity>();
                Assert.IsTrue(activities.Count > 0);
            }

            [TestMethod]
            public void AddAndRetrieveOperator()
            {
                var operator1 = new Operator()
                {
                    Name = "Activity One",
                    StudyId = 1,
                    LinkedActivitiesId = 1
                };

                var id = service.Add(operator1);

                var value = service.GetOperatorById(id);
                Assert.AreEqual(id, value.Id);
                Assert.AreEqual(value.Name, operator1.Name);
            }

            [TestMethod]
            public void GetOperators()
            {
                var operatorsList = service.GetAll<Operator>();
                Assert.IsTrue(operatorsList.Count > 0);
            }
        }
    }
}

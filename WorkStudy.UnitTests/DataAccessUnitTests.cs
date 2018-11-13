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

            private readonly IBaseRepository<ActivitySampleStudy> sampleRepo;
            private readonly IBaseRepository<Activity> activityRepo;
            private readonly IBaseRepository<Operator> operatorRepo;
            private readonly IBaseRepository<Observation> observationRepo;
            public DataAccessTests()
            {
                sampleRepo = new BaseRepository<ActivitySampleStudy>(connString);
                activityRepo = new BaseRepository<Activity>(connString);
                operatorRepo = new BaseRepository<Operator>(connString);
                observationRepo = new BaseRepository<Observation>(connString);
            }


            [TestMethod]
            public void AddAndRetrieveActivitySampleStudy()
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

                var id = sampleRepo.SaveItem(activityStudy);

                var sample = sampleRepo.GetItem(id);

                Assert.AreEqual(id, sample.Id);
                Assert.AreEqual(activityStudy.Department, sample.Department);
            }


            [TestMethod]
            public void AddAndUpdateAndRetrieveActivitySampleStudy()
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

                var id = sampleRepo.SaveItem(activityStudy);

                var sample = sampleRepo.GetItem(id);

                Assert.AreEqual(id, sample.Id);
                Assert.AreEqual(activityStudy.Department, sample.Department);

                activityStudy = new ActivitySampleStudy()
                {
                    Id = id,
                    Name = "Activity Two",
                    Date = DateTime.Now,
                    StudyNumber = 5,
                    IsRated = true,
                    StudiedBy = "Ted",
                    Department = "EngineBay"
                };

                sampleRepo.SaveItem(activityStudy);

                sample = sampleRepo.GetItem(id);

                Assert.AreEqual(id, sample.Id);
                Assert.AreEqual(activityStudy.Department, sample.Department);
            }

            [TestMethod]
            public void GetActivitySamples()
            {
                var studies = sampleRepo.GetItems();
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

                var id = activityRepo.SaveItem(activity);

                var value = activityRepo.GetItem(id);
                Assert.AreEqual(id, value.Id);
                Assert.AreEqual(value.Name, activity.Name);
            }

            [TestMethod]
            public void GetActivities()
            {
                var activities = activityRepo.GetItems();
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

                var id = operatorRepo.SaveItem(operator1);

                var value = operatorRepo.GetItem(id);
                Assert.AreEqual(id, value.Id);
                Assert.AreEqual(value.Name, operator1.Name);
            }

            [TestMethod]
            public void GetOperators()
            {
                var operatorsList = operatorRepo.GetItems();
                Assert.IsTrue(operatorsList.Count > 0);
            }

            [TestMethod]
            public void AddAndRetrieveObservation()
            {
                var observation = new Observation()
                {
                   Activity = 1,
                   Date = DateTime.Now,
                   OperatorId = 1,
                   Rating = 85
                };

                var id = observationRepo.SaveItem(observation);

                var value = observationRepo.GetItem(id);
                Assert.AreEqual(id, value.Id);
                Assert.AreEqual(value.Rating, observation.Rating);
            }

            [TestMethod]
            public void GetObservations()
            {
                var observationsList = observationRepo.GetItems();
                Assert.IsTrue(observationsList.Count > 0);
            }
        }
    }
}

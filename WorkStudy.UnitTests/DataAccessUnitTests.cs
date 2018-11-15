using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLiteNetExtensions.Extensions;
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
            private readonly IBaseRepository<OperatorActivity> operatorActivityRepo;
            private readonly IBaseRepository<MergedActivities> mergedActivityRepo;


            public DataAccessTests()
            {
                sampleRepo = new BaseRepository<ActivitySampleStudy>(connString);
                activityRepo = new BaseRepository<Activity>(connString);
                operatorRepo = new BaseRepository<Operator>(connString);
                observationRepo = new BaseRepository<Observation>(connString);
                operatorActivityRepo = new BaseRepository<OperatorActivity>(connString);
                mergedActivityRepo = new BaseRepository<MergedActivities>(connString);
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
                Assert.IsTrue(studies.ToList().Count > 0); 
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
                Assert.IsTrue(activities.ToList().Count > 0);
            }

            [TestMethod]
            public void AddAndRetrieveOperator()
            {
                var operator1 = new Operator()
                {
                    Name = "Activity One",
                    StudyId = 1000
                };

                var id = operatorRepo.SaveItem(operator1);

                var value = operatorRepo.GetItem(id);
                Assert.AreEqual(id, value.Id);
                Assert.AreEqual(value.Name, operator1.Name);
                Assert.AreEqual(value.StudyId, operator1.StudyId);
            }

            [TestMethod]
            public void GetOperators()
            {
                var operatorsList = operatorRepo.GetItems();
                Assert.IsTrue(operatorsList.ToList().Count > 0);
            }

            [TestMethod]
            public void AddAndRetrieveObservation()
            {
                var observation = new Observation()
                {
                   ActivityId = 1,
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
                Assert.IsTrue(observationsList.ToList().Count > 0);
            }

            [TestMethod]
            public void LinkMergedActivitiesToOperator()
            {
                LinkActivitiesToOperator();
            }

            [TestMethod]
            public void MergeActivitiesToActivity()
            {
                TestActivityMerges();
            }


            private Activity TestActivityMerges()
            {

                mergedActivityRepo.DatabaseConnection.CreateTable<MergedActivities>();

                var activity1 = new Activity()
                {
                    Name = "Activity One",
                    Comment = "Some comment or other",
                    Date = DateTime.Now,
                    IsEnabled = true
                };

                var activity2 = new Activity()
                {
                    Name = "Activity Two",
                    Comment = "Some comment or other",
                    Date = DateTime.Now,
                    IsEnabled = true
                };


                var activity3 = new Activity()
                {
                    Name = "Activity Three",
                    Comment = "Some comment or other",
                    Date = DateTime.Now,
                    IsEnabled = true
                };

                var activityId1 = activityRepo.SaveItem(activity1);
                var activityId2 = activityRepo.SaveItem(activity2);
                var activityId3 = activityRepo.SaveItem(activity3);

                var returnedActivity1 = activityRepo.GetItem(activityId1);
                var returnedActivity2 = activityRepo.GetItem(activityId2);
                var returnedActivity3 = activityRepo.GetItem(activityId3);

                returnedActivity1.Activities = new List<Activity> { returnedActivity2, returnedActivity3 };

                activityRepo.DatabaseConnection.UpdateWithChildren(returnedActivity1);

                var activityStored = activityRepo.DatabaseConnection.GetWithChildren<Activity>(returnedActivity1.Id);

                Assert.AreEqual(returnedActivity1.Id, activityStored.Id);
                Assert.AreEqual(returnedActivity2.Id, activityStored.Activities[0].Id);
                Assert.AreEqual(returnedActivity3.Id, activityStored.Activities[1].Id);

                return activityStored;

            }

            private Operator LinkActivitiesToOperator()
            {
                var operator1 = new Operator()
                {
                    Name = "Activity One",
                    StudyId = 1000
                };

                var operatorId = operatorRepo.SaveItem(operator1);

                var returnedOperator = operatorRepo.GetItem(operatorId);

                var activity1 = new Activity()
                {
                    Name = "Activity One",
                    Comment = "Some comment or other",
                    Date = DateTime.Now,
                    IsEnabled = true
                };


                var activity2 = new Activity()
                {
                    Name = "Activity Two",
                    Comment = "Some comment or other",
                    Date = DateTime.Now,
                    IsEnabled = true
                };

                var activityId1 = activityRepo.SaveItem(activity1);
                var activityId2 = activityRepo.SaveItem(activity2);

                var returnedActivity1 = activityRepo.GetItem(activityId1);
                var returnedActivity2 = activityRepo.GetItem(activityId2);

                operator1.Activities = new List<Activity> { returnedActivity1, returnedActivity2 };

                operatorActivityRepo.DatabaseConnection.UpdateWithChildren(operator1);

                return operator1;
            }

        }
    }
}

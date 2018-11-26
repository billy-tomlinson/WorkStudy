using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLiteNetExtensions.Extensions;
using Syncfusion.XlsIO;
using WorkStudy.Model;
using WorkStudy.Services;
using WorkStudy.ViewModels;

namespace WorkStudy.UnitTests
{
    [TestClass]
    public class DataAccessUnitTests
    {
        [TestClass]
        public class DataAccessTests
        {
            private const string connString = "WorkStudy1.db1";

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

                CleanDatabase();
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
                var activityStudy = new ActivitySampleStudy()
                {
                    Name = "Activity One",
                    Date = DateTime.Now,
                    StudyNumber = 1,
                    IsRated = true,
                    StudiedBy = "Billy",
                    Department = "PaintShop"
                };

                sampleRepo.SaveItem(activityStudy);

                var studies = sampleRepo.GetItems();
                Assert.IsTrue(studies.ToList().Count == 1);
            }

            [TestMethod]
            public void AddAndRetrieveActivity_And_Get_All_Activities()
            {

                var activity = new Activity()
                {
                    Name = "Activity One",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var id = activityRepo.SaveItem(activity);

                var value = activityRepo.GetItem(id);
                Assert.AreEqual(id, value.Id);
                Assert.AreEqual(value.Name, activity.Name);

                var activities = activityRepo.GetItems();
                Assert.IsTrue(activities.ToList().Count == 1);
            }

            [TestMethod]
            public void AddAndRetrieveOperator_And_Get_All_Operators()
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

                var operatorsList = operatorRepo.GetItems();
                Assert.IsTrue(operatorsList.ToList().Count > 0);
            }


            [TestMethod]
            public void Add_And_Retrieve_Operator_With_No_Rated_Activities()
            {
                var operator1 = Link_UnRated_Activities_To_Operator();
                Utilities.StudyId = 1000;
                var model = new AddOperatorsViewModel(connString);
                model.ValidateOperatorActivities();
                Assert.IsTrue(model.IsInvalid);

            }

            [TestMethod]
            public void Add_And_Retrieve_Operator_With_No_Rated_And_Unrated_Activities()
            {
                var operator1 = Link_UnRated_And_Rated_Activities_To_Operator();
                Utilities.StudyId = 1000;
                var model = new AddOperatorsViewModel(connString);
                model.ValidateOperatorActivities();
                Assert.IsFalse(model.IsInvalid);

            }

            [TestMethod]
            public void AddAndRetrieveObservation_And_Get_All_Observations()
            {

                var observation = new Observation()
                {
                    ActivityId = 1,
                    OperatorId = 1,
                    Rating = 85
                };

                var id = observationRepo.SaveItem(observation);

                var value = observationRepo.GetItem(id);
                Assert.AreEqual(id, value.Id);
                Assert.AreEqual(value.Rating, observation.Rating);

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

            [TestMethod]
            public void Test_Activity_Merges_Work_with_Operators()
            {

                var activity1 = new Activity()
                {
                    Name = "Activity One",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activity2 = new Activity()
                {
                    Name = "Activity Two",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };
                var activity3 = new Activity()
                {
                    Name = "Activity Three",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };


                var activity4 = new Activity()
                {
                    Name = "Activity Four",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };
                var activity5 = new Activity()
                {
                    Name = "Activity Five",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };


                var activity6 = new Activity()
                {
                    Name = "Activity Six",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activities = new List<Activity>()
                {
                    activity1, activity2 , activity3 , activity4 , activity5 , activity6
                };

                foreach (var item in activities)
                {
                    activityRepo.SaveItem(item);
                }

                activities = activityRepo.GetAllWithChildren().ToList();

                var operator1 = new Operator()
                {
                    Name = "Operator One",
                    StudyId = 1000,
                    Activities = new List<Activity>() { activities[0], activities[1] , activities[2] }
                };

                var operator2 = new Operator()
                {
                    Name = "Operator Two",
                    StudyId = 1000,
                    Activities = new List<Activity>() { activities[3], activities[4], activities[5] }
                };

                operatorRepo.SaveItem(operator1);
                operatorRepo.SaveItem(operator2);
                operatorRepo.UpdateWithChildren(operator1);
                operatorRepo.UpdateWithChildren(operator2);

                var operators = operatorRepo.GetAllWithChildren().ToList();

                //merge Activity One and Activity Four. Activity Four will be disabled
                var mergeModel = new EditActivitiesViewModel(connString);
                mergeModel.MergedActivities.Add(activities[0]);
                mergeModel.MergedActivities.Add(activities[3]);
                mergeModel.SaveActivityDetails();


                //activities should not have Activity Four
                activities = activityRepo.GetAllWithChildren().ToList();

                operators = mergeModel.OperatorRepo.GetAllWithChildren().ToList();

                foreach (var item in operators)
                {
                    var v = mergeModel.OperatorRepo.GetWithChildren(item.Id);
                }

            }

            [TestMethod]
            public void Create_Excel_Spreadsheet_From_SQL()
            {
                var activity1 = new Activity()
                {
                    Name = "Activity One",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activity2 = new Activity()
                {
                    Name = "Activity Two",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };
                var activity3 = new Activity()
                {
                    Name = "Activity Three",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };


                var activity4 = new Activity()
                {
                    Name = "Activity Four",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };
                var activity5 = new Activity()
                {
                    Name = "Activity Five",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };


                var activity6 = new Activity()
                {
                    Name = "Activity Six",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activities = new List<Activity>()
                {
                    activity1, activity2 , activity3 , activity4 , activity5 , activity6
                };

                foreach (var item in activities)
                {
                    activityRepo.SaveItem(item);
                }

                var x = activityRepo.GetItems();

                using (ExcelEngine excelEngine = new ExcelEngine())
                {

                    //Set the default application version as Excel 2013.
                    excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;

                    //Create a workbook with a worksheet
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                    //Access first worksheet from the workbook instance.
                    IWorksheet worksheet = workbook.Worksheets[0];

                    worksheet.ImportData(x, 1, 1, true);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        workbook.Close();

                        ms.Seek(0, SeekOrigin.Begin);

                        using (FileStream fs = new FileStream("output.xlsx", FileMode.OpenOrCreate))
                        {
                            ms.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }

            [TestMethod]
            public void Add_And_RetrieveOperator_And_Link_To_UnRated_Activities()
            {
                Utilities.StudyId = 1000;

                var studyModel = new StudyDetailsViewModel(connString);
                studyModel.CreateUnratedActivities();

                var operator1 = new Operator()
                {
                    Name = "Activity One",
                    StudyId = Utilities.StudyId
                };

                var operator2 = new Operator()
                {
                    Name = "Activity Two",
                    StudyId = Utilities.StudyId
                };

                var id1 = operatorRepo.SaveItem(operator1);
                var id2 = operatorRepo.SaveItem(operator2);

                var model = new AddOperatorsViewModel(connString);
                model.LinkAllOperatorsToUnratedActivities();

                var ops = operatorRepo.GetAllWithChildren().Where(x => x.StudyId == Utilities.StudyId).ToList();

            }
            private Activity TestActivityMerges()
            {

                mergedActivityRepo.DatabaseConnection.CreateTable<MergedActivities>();

                var activity1 = new Activity()
                {
                    Name = "Activity One",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activity2 = new Activity()
                {
                    Name = "Activity Two",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };


                var activity3 = new Activity()
                {
                    Name = "Activity Three",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activityId1 = activityRepo.SaveItem(activity1);
                var activityId2 = activityRepo.SaveItem(activity2);
                var activityId3 = activityRepo.SaveItem(activity3);

                var returnedActivity1 = activityRepo.GetItem(activityId1);
                var returnedActivity2 = activityRepo.GetItem(activityId2);
                var returnedActivity3 = activityRepo.GetItem(activityId3);

                returnedActivity1.Activities = new List<Activity> {returnedActivity2, returnedActivity3};

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
                    IsEnabled = true
                };


                var activity2 = new Activity()
                {
                    Name = "Activity Two",
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activityId1 = activityRepo.SaveItem(activity1);
                var activityId2 = activityRepo.SaveItem(activity2);

                var returnedActivity1 = activityRepo.GetItem(activityId1);
                var returnedActivity2 = activityRepo.GetItem(activityId2);

                operator1.Activities = new List<Activity> {returnedActivity1, returnedActivity2};

                operatorActivityRepo.DatabaseConnection.UpdateWithChildren(operator1);

                return operator1;
            }

            private Operator Link_UnRated_Activities_To_Operator()
            {
                var operator1 = new Operator()
                {
                    Name = "Operator One",
                    StudyId = 1000
                };

                var operatorId = operatorRepo.SaveItem(operator1);

                var returnedOperator = operatorRepo.GetItem(operatorId);

                var activity1 = new Activity()
                {
                    Name = "Inactive One",
                    Comment = "Some comment or other",
                    IsEnabled = true,
                    Rated = false
                };


                var activity2 = new Activity()
                {
                    Name = "Inactive Two",
                    Comment = "Some comment or other",
                    IsEnabled = true,
                    Rated = false
                };

                var activityId1 = activityRepo.SaveItem(activity1);
                var activityId2 = activityRepo.SaveItem(activity2);

                var returnedActivity1 = activityRepo.GetItem(activityId1);
                var returnedActivity2 = activityRepo.GetItem(activityId2);

                operator1.Activities = new List<Activity> { returnedActivity1, returnedActivity2 };

                operatorActivityRepo.DatabaseConnection.UpdateWithChildren(operator1);

                return operator1;
            }

            private Operator Link_UnRated_And_Rated_Activities_To_Operator()
            {
                var operator1 = new Operator()
                {
                    Name = "Operator One",
                    StudyId = 1000
                };

                var operatorId = operatorRepo.SaveItem(operator1);

                var returnedOperator = operatorRepo.GetItem(operatorId);

                var activity1 = new Activity()
                {
                    Name = "Inactive One",
                    Comment = "Some comment or other",
                    IsEnabled = true,
                    Rated = false
                };


                var activity2 = new Activity()
                {
                    Name = "Rated Two",
                    Comment = "Some comment or other",
                    IsEnabled = true,
                    Rated = true
                };

                var activityId1 = activityRepo.SaveItem(activity1);
                var activityId2 = activityRepo.SaveItem(activity2);

                var returnedActivity1 = activityRepo.GetItem(activityId1);
                var returnedActivity2 = activityRepo.GetItem(activityId2);

                operator1.Activities = new List<Activity> { returnedActivity1, returnedActivity2 };

                operatorActivityRepo.DatabaseConnection.UpdateWithChildren(operator1);

                return operator1;
            }

            private void CleanDatabase()
            {
                activityRepo.DatabaseConnection.DropTable<Activity>();
                activityRepo.DatabaseConnection.CreateTable<Activity>();

                operatorRepo.DatabaseConnection.DropTable<Operator>();
                operatorRepo.DatabaseConnection.CreateTable<Operator>();

                sampleRepo.DatabaseConnection.DropTable<ActivitySampleStudy>();
                sampleRepo.DatabaseConnection.CreateTable<ActivitySampleStudy>();

                observationRepo.DatabaseConnection.DropTable<Observation>();
                observationRepo.DatabaseConnection.CreateTable<Observation>();

                operatorActivityRepo.DatabaseConnection.DropTable<OperatorActivity>();
                operatorActivityRepo.DatabaseConnection.CreateTable<OperatorActivity>();

                mergedActivityRepo.DatabaseConnection.DropTable<MergedActivities>();
                mergedActivityRepo.DatabaseConnection.CreateTable<MergedActivities>();
            }


        }
    }
}


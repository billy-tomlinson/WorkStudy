using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            private const string connString = "/Users/billytomlinson/WorkStudy1.db3";
            //private const string connString = "WorkStudy1.db3";

            private readonly IBaseRepository<ActivitySampleStudy> sampleRepo;
            private readonly IBaseRepository<Activity> activityRepo;
            private readonly IBaseRepository<Operator> operatorRepo;
            private readonly IBaseRepository<Observation> observationRepo;
            //private readonly IBaseRepository<OperatorActivity> operatorActivityRepo;
            private readonly IBaseRepository<MergedActivities> mergedActivityRepo;

            public DataAccessTests()
            {
                sampleRepo = new BaseRepository<ActivitySampleStudy>(connString);
                activityRepo = new BaseRepository<Activity>(connString);
                operatorRepo = new BaseRepository<Operator>(connString);
                observationRepo = new BaseRepository<Observation>(connString);
                //operatorActivityRepo = new BaseRepository<OperatorActivity>(connString);
                mergedActivityRepo = new BaseRepository<MergedActivities>(connString);

                //CleanDatabase();
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
                    ActivityName = new ActivityName() { Name = "Activity One"},
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var id = activityRepo.SaveItem(activity);

                var value = activityRepo.GetItem(id);
                Assert.AreEqual(id, value.Id);
                Assert.AreEqual(value.ActivityName.Name, activity.ActivityName.Name);

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


            //[TestMethod]
            //public void Add_And_Retrieve_Operator_With_No_Rated_Activities()
            //{
            //    var operator1 = Link_UnRated_Activities_To_Operator();
            //    Utilities.StudyId = 1000;
            //    var model = new AddOperatorsViewModel(connString);
            //    model.ValidateOperatorActivities();
            //    Assert.IsTrue(model.IsInvalid);

            //}

            [TestMethod]
            public void Add_And_Retrieve_Operator_With_No_Rated_And_Unrated_Activities()
            {
                Utilities.StudyId = 1000;
                Link_UnRated_And_Rated_Activities_To_Operator();

                var studyOperators = operatorRepo.GetAllWithChildren()
                                         .Where(_ => _.StudyId == Utilities.StudyId).ToList();

                //if (studyOperators.Any(_ => !_.Activities.Any(x => x.Rated)))
                //{
                //    Assert.IsTrue(studyOperators.Count()> 0);
                //}

                CleanDatabase();

                Link_UnRated_Activities_To_Operators();

                studyOperators = operatorRepo.GetAllWithChildren()
                                         .Where(_ => _.StudyId == Utilities.StudyId).ToList();

                //if (studyOperators.Any(_ => !_.Activities.Any(x => x.Rated)))
                //{
                //    Assert.IsTrue(studyOperators.Count() > 0);
                //}
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

                observation = new Observation()
                {
                    ActivityId = 2,
                    OperatorId = 1,
                    Rating = 85
                };

                id = observationRepo.SaveItem(observation);

                observation = new Observation()
                {
                    ActivityId = 1,
                    OperatorId = 1,
                    Rating = 85
                };

                id = observationRepo.SaveItem(observation);

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
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activity2 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };
                var activity3 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };


                var activity4 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };
                var activity5 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };


                var activity6 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
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

                //var operator1 = new Operator()
                //{
                //    Name = "Operator One",
                //    StudyId = 1000,
                //    Activities = new List<Activity>() { activities[0], activities[1] , activities[2] }
                //};

                //var operator2 = new Operator()
                //{
                //    Name = "Operator Two",
                //    StudyId = 1000,
                //    Activities = new List<Activity>() { activities[3], activities[4], activities[5] }
                //};

                //operatorRepo.SaveItem(operator1);
                //operatorRepo.SaveItem(operator2);
                //operatorRepo.UpdateWithChildren(operator1);
                //operatorRepo.UpdateWithChildren(operator2);

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
                #region
                var activity1 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activity2 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };
                var activity3 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };


                var activity4 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };
                var activity5 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };


                var activity6 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activities = new List<Activity>()
                {
                    activity1, activity2 , activity3 , activity4 , activity5 , activity6
                };

                foreach (var item in activities)
                {
                    //activityRepo.SaveItem(item);
                }

                //var x = activityRepo.GetItems();

                //AddAndUpdateAndRetrieveActivitySampleStudy();
                //AddAndRetrieveOperator_And_Get_All_Operators();
                //AddAndRetrieveObservation_And_Get_All_Observations();

                var operators = operatorRepo.GetAllWithChildren().Where(cw => cw.StudyId == 15).ToList();

                //var operators = activityRepo.GetItems().ToList();
                #endregion
                var sample = sampleRepo.GetItem(15);

                using (ExcelEngine excelEngine = new ExcelEngine())
                {

                    var allActivities = activityRepo.GetItems().Where(x => x.StudyId == 15 && x.Rated)
                        .Select(y => new ActivityName() { Name = y.ActivityName.Name }).ToList();

                    //Set the default application version as Excel 2013.
                    excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;

                    //Create a workbook with a worksheet
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
                    var destSheetAll = workbook.Worksheets.Create("Summary");

                    BuildRatedActivities(operators, sample, workbook, destSheetAll);
                    BuildUnRatedActivities(operators, destSheetAll);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        workbook.Close();

                        ms.Seek(0, SeekOrigin.Begin);

                        using (FileStream fs = new FileStream("ReportOutputTestSQLSummaryQQ.xlsx", FileMode.OpenOrCreate))
                        {
                            ms.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }

            private void BuildRatedActivities(List<Operator> operators, ActivitySampleStudy sample, IWorkbook workbook, IWorksheet destSheetAll)
            {
                List<List<ObservationSummary>> allTotals = new List<List<ObservationSummary>>();

                var allActivities = activityRepo.GetItems().Where(x => x.StudyId == 15 && x.Rated)
                .Select(y => new ActivityName() { Name = y.ActivityName.Name }).ToList();

                destSheetAll.Range[3, 1].Text = "Activity";
                destSheetAll.ImportData(allActivities, 5, 1, false);

                var totalObs = observationRepo.GetItems().Where(x => x.StudyId == 15).ToList();
                var totalCount = totalObs.Count();
                var firstOb = totalObs.Min(y => y.Date);
                var lastOb = totalObs.Max(y => y.Date);
                var totalTimeMinutes = lastOb.Subtract(firstOb).TotalMinutes;
                var timePerObservation = Math.Round(totalTimeMinutes / totalCount, 2);

                foreach (var op in operators)
                {
                    var data = new List<SpreadSheetObservation>();
                    var obs = totalObs.Where(x => x.OperatorId == op.Id).ToList();
                    var totalObsPerOperator = obs.Count();
                    foreach (var observation in obs)
                    {
                        var formattedDate = String.Format("{0:d/M/yyyy HH:mm:ss}", observation.Date);
                        data.Add(new SpreadSheetObservation()
                        {
                            ActivityName = observation.ActivityName,
                            Study = sample.Name,
                            OperatorName = op.Name,
                            ObservationNumber = observation.ObservationNumber,
                            Rating = observation.Rating,
                            Date = formattedDate
                        });
                    }
                    var destSheet = workbook.Worksheets.Create(op.Name);

                    destSheet.Range["A1"].Text = "Study";
                    destSheet.Range["B1"].Text = "Date";
                    destSheet.Range["C1"].Text = "Operator";
                    destSheet.Range["D1"].Text = "Observation Round";
                    destSheet.Range["E1"].Text = "Activity";
                    destSheet.Range["F1"].Text = "Rating";

                    destSheet.ImportData(data, 3, 1, false);

                    var summary = obs.GroupBy(a => new { a.ActivityId, a.ActivityName })
                        .Select(g => new ObservationSummary
                        {
                            ActivityName = g.Key.ActivityName,
                            NumberOfObservations = g.Count()
                        }).ToList();

                    foreach (var item in summary)
                    {
                        var totalPercentage = Math.Round((double)item.NumberOfObservations / totalObsPerOperator * 100, 2);
                        item.Percentage = totalPercentage;
                        item.TotalTime = item.NumberOfObservations * timePerObservation;
                        item.OperatorName = op.Name;
                    }

                    allTotals.Add(summary);
                }

                var columnCount = 1;

                var computedRange = $"A5:A{allActivities.Count + 5}";
                var range = destSheetAll[computedRange].ToList();

                foreach (var item in allTotals)
                {
                    destSheetAll.Range[3, columnCount + 2].Text = "Total Obs";
                    destSheetAll.Range[3, columnCount + 3].Text = "Total Time";
                    destSheetAll.Range[3, columnCount + 4].Text = "% of Total";

                    foreach (var cell in range.Where(x => x.Value != string.Empty))
                    {

                        var v = cell.Value;
                        var c = cell.Row;

                        foreach (var vv in item)
                        {
                            destSheetAll.Range[1, columnCount + 2].Text = vv.OperatorName;

                            if (vv.ActivityName == v)
                            {
                                destSheetAll.Range[c, columnCount + 2].Number = vv.NumberOfObservations;
                                destSheetAll.Range[c, columnCount + 3].Number = vv.TotalTime;
                                destSheetAll.Range[c, columnCount + 4].Number = vv.Percentage;
                            }
                        }
                    }

                    var columnAddress1 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                    var columnAddress2 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);
                    var columnAddress3 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 4].AddressLocal, @"[\d-]", string.Empty);

                    var formula1 = $"=SUM({columnAddress1}5:{columnAddress1}{allActivities.Count + 5})";
                    var formula2 = $"=SUM({columnAddress2}5:{columnAddress2}{allActivities.Count + 5})";
                    var formula3 = $"=SUM({columnAddress3}5:{columnAddress3}{allActivities.Count + 5})";

                    destSheetAll.Range[allActivities.Count + 6, columnCount + 2].Formula = formula1;
                    destSheetAll.Range[allActivities.Count + 6, columnCount + 3].Formula = formula2;
                    destSheetAll.Range[allActivities.Count + 6, columnCount + 4].Formula = formula3;

                    columnCount = columnCount + 5;
                }

                destSheetAll.Range[3, columnCount + 1].Text = "OBS TOTAL";
                destSheetAll.Range[3, columnCount + 2].Text = "TIME TOTAL";
                destSheetAll.Range[3, columnCount + 3].Text = "% TOTAL";


                foreach (var item in allTotals)
                {
                    foreach (var cell in range.Where(x => x.Value != string.Empty))
                    {
                        var v = cell.Value;
                        var c = cell.Row;

                        foreach (var vv in item)
                        {
                            if (vv.ActivityName == v)
                            {
                                var totalActivity = totalObs.Count(x => x.ActivityName == v);
                                var totalObsCount = totalObs.Count();
                                var totalPercent = Math.Round((double)totalActivity / totalObsCount * 100, 2);
                                var totalPerActivity = Math.Round((double)totalTimeMinutes / totalActivity, 2);

                                destSheetAll.Range[c, columnCount + 1].Number = Math.Round((double)totalActivity, 2);
                                destSheetAll.Range[c, columnCount + 2].Number = Math.Round((double)totalPerActivity, 2);
                                destSheetAll.Range[c, columnCount + 3].Number = Math.Round((double)totalPercent, 2); ;
                            }
                        }
                    }

                    var columnAddress1 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 1].AddressLocal, @"[\d-]", string.Empty);
                    var columnAddress2 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                    var columnAddress3 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);

                    var formula1 = $"=SUM({columnAddress1}5:{columnAddress1}{allActivities.Count + 5})";
                    var formula2 = $"=SUM({columnAddress2}5:{columnAddress2}{allActivities.Count + 5})";
                    var formula3 = $"=SUM({columnAddress3}5:{columnAddress3}{allActivities.Count + 5})";

                    destSheetAll.Range[allActivities.Count + 6, columnCount + 1].Formula = formula1;
                    destSheetAll.Range[allActivities.Count + 6, columnCount + 2].Formula = formula2;
                    destSheetAll.Range[allActivities.Count + 6, columnCount + 3].Formula = formula3;
                }
            }

            private void BuildUnRatedActivities(List<Operator> operators, IWorksheet destSheetAll)
            {
                List<List<ObservationSummary>> allTotals = new List<List<ObservationSummary>>();

                var allActivities = activityRepo.GetItems().Where(x => x.StudyId == 15 && !x.Rated)
                .Select(y => new ActivityName() { Name = y.ActivityName.Name }).ToList();

                destSheetAll.ImportData(allActivities, 12, 1, false);

                var totalObs = observationRepo.GetItems().Where(x => x.StudyId == 15).ToList();
                var totalCount = totalObs.Count();
                var firstOb = totalObs.Min(y => y.Date);
                var lastOb = totalObs.Max(y => y.Date);
                var totalTimeMinutes = lastOb.Subtract(firstOb).TotalMinutes;
                var timePerObservation = Math.Round(totalTimeMinutes / totalCount, 2);

                foreach (var op in operators)
                {
                    var data = new List<SpreadSheetObservation>();
                    var obs = totalObs.Where(x => x.OperatorId == op.Id).ToList();
                    var totalObsPerOperator = obs.Count();

                    var summary = obs.GroupBy(a => new { a.ActivityId, a.ActivityName })
                        .Select(g => new ObservationSummary
                        {
                            ActivityName = g.Key.ActivityName,
                            NumberOfObservations = g.Count()
                        }).ToList();

                    foreach (var item in summary)
                    {
                        var totalPercentage = Math.Round((double)item.NumberOfObservations / totalObsPerOperator * 100, 2);
                        item.Percentage = totalPercentage;
                        item.TotalTime = item.NumberOfObservations * timePerObservation;
                        item.OperatorName = op.Name;
                    }

                    allTotals.Add(summary);
                }

                var columnCount = 1;

                var computedRange = $"A12:A{allActivities.Count + 12}";
                var range = destSheetAll[computedRange].ToList();

                foreach (var item in allTotals)
                {
                    foreach (var cell in range.Where(x => x.Value != string.Empty))
                    {

                        var v = cell.Value;
                        var c = cell.Row;

                        foreach (var vv in item)
                        {
                            if (vv.ActivityName == v)
                            {
                                destSheetAll.Range[c, columnCount + 2].Number = vv.NumberOfObservations;
                                destSheetAll.Range[c, columnCount + 3].Number = vv.TotalTime;
                                destSheetAll.Range[c, columnCount + 4].Number = vv.Percentage;
                            }
                        }
                    }

                    var columnAddress1 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                    var columnAddress2 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);
                    var columnAddress3 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 4].AddressLocal, @"[\d-]", string.Empty);

                    var formula1 = $"=SUM({columnAddress1}12:{columnAddress1}{allActivities.Count + 11})";
                    var formula2 = $"=SUM({columnAddress2}12:{columnAddress2}{allActivities.Count + 11})";
                    var formula3 = $"=SUM({columnAddress3}12:{columnAddress3}{allActivities.Count + 11})";

                    destSheetAll.Range[allActivities.Count + 12, columnCount + 2].Formula = formula1;
                    destSheetAll.Range[allActivities.Count + 12, columnCount + 3].Formula = formula2;
                    destSheetAll.Range[allActivities.Count + 12, columnCount + 4].Formula = formula3;

                    var columnAddress4 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 1].AddressLocal, @"[\d-]", string.Empty);
                    var columnAddress5 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                    var columnAddress6 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);

                    var formula4 = $"=SUM({columnAddress1}9+{columnAddress1}15)";
                    var formula5 = $"=SUM({columnAddress2}9+{columnAddress2}15)";
                    var formula6 = $"=SUM({columnAddress3}9+{columnAddress3}15)";

                    destSheetAll.Range[17, columnCount + 2].Formula = formula4;
                    destSheetAll.Range[17, columnCount + 3].Formula = formula5;
                    destSheetAll.Range[17, columnCount + 4].Formula = formula6;

                    columnCount = columnCount + 5;
                }

                foreach (var item in allTotals)
                {
                    foreach (var cell in range.Where(x => x.Value != string.Empty))
                    {
                        var v = cell.Value;
                        var c = cell.Row;

                        foreach (var vv in item)
                        {
                            if (vv.ActivityName == v)
                            {
                                var totalActivity = totalObs.Count(x => x.ActivityName == v);
                                var totalObsCount = totalObs.Count();
                                var totalPercent = Math.Round((double)totalActivity / totalObsCount * 100, 2);
                                var totalPerActivity = Math.Round((double)totalTimeMinutes / totalActivity, 2);

                                destSheetAll.Range[c, columnCount + 1].Number = Math.Round((double)totalActivity, 2);
                                destSheetAll.Range[c, columnCount + 2].Number = Math.Round((double)totalPerActivity, 2);
                                destSheetAll.Range[c, columnCount + 3].Number = Math.Round((double)totalPercent, 2); ;
                            }
                        }
                    }

                    var columnAddress1 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 1].AddressLocal, @"[\d-]", string.Empty);
                    var columnAddress2 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                    var columnAddress3 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);

                    var formula1 = $"=SUM({columnAddress1}12:{columnAddress1}14)";
                    var formula2 = $"=SUM({columnAddress2}12:{columnAddress2}14)";
                    var formula3 = $"=SUM({columnAddress3}12:{columnAddress3}14)";

                    destSheetAll.Range[15, columnCount + 1].Formula = formula1;
                    destSheetAll.Range[15, columnCount + 2].Formula = formula2;
                    destSheetAll.Range[15, columnCount + 3].Formula = formula3;

                    //all totals
                    var columnAddress4 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 1].AddressLocal, @"[\d-]", string.Empty);
                    var columnAddress5 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                    var columnAddress6 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);

                    var formula4 = $"=SUM({columnAddress1}9+{columnAddress1}15)";
                    var formula5 = $"=SUM({columnAddress2}9+{columnAddress2}15)";
                    var formula6 = $"=SUM({columnAddress3}9+{columnAddress3}15)";

                    destSheetAll.Range[17, columnCount + 1].Formula = formula4;
                    destSheetAll.Range[17, columnCount + 2].Formula = formula5;
                    destSheetAll.Range[17, columnCount + 3].Formula = formula6;

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
                //model.LinkAllOperatorsToUnratedActivities();

                var ops = operatorRepo.GetAllWithChildren().Where(x => x.StudyId == Utilities.StudyId).ToList();

            }
            private Activity TestActivityMerges()
            {

                mergedActivityRepo.CreateTable();

                var activity1 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activity2 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };


                var activity3 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activityId1 = activityRepo.SaveItem(activity1);
                var activityId2 = activityRepo.SaveItem(activity2);
                var activityId3 = activityRepo.SaveItem(activity3);

                var returnedActivity1 = activityRepo.GetItem(activityId1);
                var returnedActivity2 = activityRepo.GetItem(activityId2);
                var returnedActivity3 = activityRepo.GetItem(activityId3);

                returnedActivity1.Activities = new List<Activity> { returnedActivity2, returnedActivity3 };

                activityRepo.UpdateWithChildren(returnedActivity1);

                var activityStored = activityRepo.GetWithChildren(returnedActivity1.Id);

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
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };


                var activity2 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true
                };

                var activityId1 = activityRepo.SaveItem(activity1);
                var activityId2 = activityRepo.SaveItem(activity2);

                var returnedActivity1 = activityRepo.GetItem(activityId1);
                var returnedActivity2 = activityRepo.GetItem(activityId2);

                //operator1.Activities = new List<Activity> {returnedActivity1, returnedActivity2};

                //operatorActivityRepo.UpdateWithChildren(operator1);

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
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true,
                    Rated = false
                };


                var activity2 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true,
                    Rated = false
                };

                var activityId1 = activityRepo.SaveItem(activity1);
                var activityId2 = activityRepo.SaveItem(activity2);

                var returnedActivity1 = activityRepo.GetItem(activityId1);
                var returnedActivity2 = activityRepo.GetItem(activityId2);

                //operator1.Activities = new List<Activity> { returnedActivity1, returnedActivity2 };

                //operatorActivityRepo.UpdateWithChildren(operator1);

                return operator1;
            }

            private void Link_UnRated_And_Rated_Activities_To_Operator()
            {
                var operator1 = new Operator()
                {
                    Name = "Operator One",
                    StudyId = Utilities.StudyId
                };


                var operator2 = new Operator()
                {
                    Name = "Operator Two",
                    StudyId = Utilities.StudyId
                };

                var operatorId1 = operatorRepo.SaveItem(operator1);
                var operatorId2 = operatorRepo.SaveItem(operator2);

                var returnedOperator1 = operatorRepo.GetItem(operatorId1);
                var returnedOperator2 = operatorRepo.GetItem(operatorId2);

                var activity1 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true,
                    Rated = false
                };


                var activity2 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true,
                    Rated = true
                };

                var activityId1 = activityRepo.SaveItem(activity1);
                var activityId2 = activityRepo.SaveItem(activity2);

                var returnedActivity1 = activityRepo.GetItem(activityId1);
                var returnedActivity2 = activityRepo.GetItem(activityId2);

                //operator1.Activities = new List<Activity> { returnedActivity1, returnedActivity2 };
                //operator2.Activities = new List<Activity> { returnedActivity1 };

                //operatorActivityRepo.UpdateWithChildren(operator1);
                //operatorActivityRepo.UpdateWithChildren(operator2);
            }


            private void Link_UnRated_Activities_To_Operators()
            {
                var operator1 = new Operator()
                {
                    Name = "Operator One",
                    StudyId = Utilities.StudyId
                };


                var operator2 = new Operator()
                {
                    Name = "Operator Two",
                    StudyId = Utilities.StudyId
                };

                var operatorId1 = operatorRepo.SaveItem(operator1);
                var operatorId2 = operatorRepo.SaveItem(operator2);

                var returnedOperator1 = operatorRepo.GetItem(operatorId1);
                var returnedOperator2 = operatorRepo.GetItem(operatorId2);

                var activity1 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true,
                    Rated = false
                };


                var activity2 = new Activity()
                {
                    ActivityName = new ActivityName() { Name = "Activity One" },
                    Comment = "Some comment or other",
                    IsEnabled = true,
                    Rated = false
                };

                var activityId1 = activityRepo.SaveItem(activity1);
                var activityId2 = activityRepo.SaveItem(activity2);

                var returnedActivity1 = activityRepo.GetItem(activityId1);
                var returnedActivity2 = activityRepo.GetItem(activityId2);

                //operator1.Activities = new List<Activity> { returnedActivity1, returnedActivity2 };
                //operator2.Activities = new List<Activity> { returnedActivity1 };

                //operatorActivityRepo.UpdateWithChildren(operator1);
                //operatorActivityRepo.UpdateWithChildren(operator2);
            }
            private void CleanDatabase()
            {
                activityRepo.DropTable();
                activityRepo.CreateTable();

                operatorRepo.DropTable();
                operatorRepo.CreateTable();

                sampleRepo.DropTable();
                sampleRepo.CreateTable();

                observationRepo.DropTable();
                observationRepo.CreateTable();

                mergedActivityRepo.DropTable();
                mergedActivityRepo.CreateTable();
            }

            //[TestMethod]
            //public void Test_Operator_Activities_SumUp()
            //{
            //    List<OperatorRunningTotal> runningTotals = new List<OperatorRunningTotal>();

            //    int opId = 1;

            //    var operators = operatorRepo.GetWithChildren(opId);
            //    //var activities = operators.Activities;
            //    var observations = operators.Observations;
            //    var totalObs = observations.Count;

            //    //foreach (var item in activities)
            //    //{
            //    //    var count = observations.Count(x => x.ActivityId == item.Id);
            //    //    double percentage = count > 0 ? (double)count / totalObs : 0;
            //    //    percentage = Math.Round(percentage * 100, 1); 

            //    //    var runningTotal = new OperatorRunningTotal()
            //    //    {
            //    //        ActivityId = item.Id,
            //    //        OperatorId = opId,
            //    //        ActivityName = item.Name,
            //    //        NumberOfObservations = count,
            //    //        Percentage = percentage,
            //    //        PercentageFormatted = $"{percentage.ToString(CultureInfo.InvariantCulture)}%"
            //    //    };

            //    //    runningTotals.Add(runningTotal);
            //    //}
            //}

        }
    }
}

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
    public class ExcelTests
    {
        //private const string connString = "/Users/billytomlinson/WorkStudy1.db3";
        private const string connString = "WorkStudy1.db3";

        private readonly IBaseRepository<ActivitySampleStudy> sampleRepo;
        private readonly IBaseRepository<Activity> activityRepo;
        private readonly IBaseRepository<Operator> operatorRepo;
        private readonly IBaseRepository<Observation> observationRepo;

        List<Operator> operators;
        ActivitySampleStudy sample;
        List<Activity> allStudyActivities;
        List<Observation> totalObs;
        List<List<ObservationSummary>> allTotals;

        double timePerObservation;
        double totalTimeMinutes;

        IWorkbook workbook;
        IWorksheet destSheetAll;

        int ratedActivitiesCount;
        int unRatedActivitiesCount;

        int ratedActivitiesTotalRowIndex;
        int unRatedActivitiesTotalRowIndex;

        public ExcelTests()
        {
            Utilities.StudyId = 1;
            sampleRepo = new BaseRepository<ActivitySampleStudy>(connString);
            activityRepo = new BaseRepository<Activity>(connString);
            operatorRepo = new BaseRepository<Operator>(connString);
            observationRepo = new BaseRepository<Observation>(connString);

            operators = operatorRepo.GetAllWithChildren().Where(cw => cw.StudyId == Utilities.StudyId).ToList();
            sample = sampleRepo.GetItem(Utilities.StudyId);
            allStudyActivities = activityRepo.GetItems().Where(x => x.StudyId == Utilities.StudyId).ToList();

            totalObs = observationRepo.GetItems().Where(x => x.StudyId == Utilities.StudyId).ToList();
            var totalCount = totalObs.Count();
            var firstOb = totalObs.Min(y => y.Date);
            var lastOb = totalObs.Max(y => y.Date);
            totalTimeMinutes = lastOb.Subtract(firstOb).TotalMinutes;
            timePerObservation = Math.Round(totalTimeMinutes / totalCount, 2);
        }

        [TestMethod]
        public void Create_Excel_Spreadsheet_From_SQL()
        {

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Set the default application version as Excel 2013.
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;

                //Create a workbook with a worksheet
                workbook = excelEngine.Excel.Workbooks.Create(1);
                destSheetAll = workbook.Worksheets.Create("Summary");

                BuildRatedActivities();
                BuildUnRatedActivities();

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

        private void BuildRatedActivities()
        {
            allTotals = new List<List<ObservationSummary>>();

            var allActivities = allStudyActivities.Where(x => x.Rated)
                .Select(y => new ActivityName() { Name = y.Name }).ToList();

            ratedActivitiesCount = allActivities.Count;

            destSheetAll.Range[3, 1].Text = "Activity";
            destSheetAll.ImportData(allActivities, 5, 1, false);

            CreateSheetPerOperator();

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

                ratedActivitiesTotalRowIndex = allActivities.Count + 6;

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
                            destSheetAll.Range[c, columnCount + 3].Number = Math.Round((double)totalPercent, 2); 
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

        private void BuildUnRatedActivities()
        {
            allTotals = new List<List<ObservationSummary>>();

            var allActivities = allStudyActivities.Where(x => !x.Rated)
             .Select(y => new ActivityName() { Name = y.Name }).ToList();

            unRatedActivitiesCount = allActivities.Count;

            var unratedStartRow = ratedActivitiesTotalRowIndex + 2;

            destSheetAll.ImportData(allActivities, unratedStartRow, 1, false);

            foreach (var op in operators)
            {
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

            var computedRange = $"A{unratedStartRow}:A{unratedStartRow + allActivities.Count}";
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

                // Total All Unrated observations
                var columnAddress1 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress2 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress3 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 4].AddressLocal, @"[\d-]", string.Empty);

                var formula1 = $"=SUM({columnAddress1}{unratedStartRow}:{columnAddress1}{allActivities.Count + unratedStartRow})";
                var formula2 = $"=SUM({columnAddress2}{unratedStartRow}:{columnAddress2}{allActivities.Count + unratedStartRow})";
                var formula3 = $"=SUM({columnAddress3}{unratedStartRow}:{columnAddress3}{allActivities.Count + unratedStartRow})";

                destSheetAll.Range[allActivities.Count + unratedStartRow + 1, columnCount + 2].Formula = formula1;
                destSheetAll.Range[allActivities.Count + unratedStartRow + 1, columnCount + 3].Formula = formula2;
                destSheetAll.Range[allActivities.Count + unratedStartRow + 1, columnCount + 4].Formula = formula3;

                unRatedActivitiesTotalRowIndex = allActivities.Count + unratedStartRow + 1;

                // Total All observations  - Add together total Rated +  total unrated
                var formula4 = $"=SUM({columnAddress1}{ratedActivitiesTotalRowIndex}+{columnAddress1}{unRatedActivitiesTotalRowIndex})";
                var formula5 = $"=SUM({columnAddress2}{ratedActivitiesTotalRowIndex}+{columnAddress2}{unRatedActivitiesTotalRowIndex})";
                var formula6 = $"=SUM({columnAddress3}{ratedActivitiesTotalRowIndex}+{columnAddress3}{unRatedActivitiesTotalRowIndex})";

                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 2].Formula = formula4;
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 3].Formula = formula5;
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 4].Formula = formula6;

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
                            destSheetAll.Range[c, columnCount + 3].Number = Math.Round((double)totalPercent, 2);
                        }
                    }
                }

                //var columnAddress1 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 1].AddressLocal, @"[\d-]", string.Empty);
                //var columnAddress2 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                //var columnAddress3 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);

                //var formula1 = $"=SUM({columnAddress1}12:{columnAddress1}13)";
                //var formula2 = $"=SUM({columnAddress2}12:{columnAddress2}13)";
                //var formula3 = $"=SUM({columnAddress3}12:{columnAddress3}13)";

                //destSheetAll.Range[15, columnCount + 1].Formula = formula1;
                //destSheetAll.Range[15, columnCount + 2].Formula = formula2;
                //destSheetAll.Range[15, columnCount + 3].Formula = formula3;

                //all totals
                //var columnAddress4 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 1].AddressLocal, @"[\d-]", string.Empty);
                //var columnAddress5 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                //var columnAddress6 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);

                //var formula4 = $"=SUM({columnAddress1}9+{columnAddress1}15)";
                //var formula5 = $"=SUM({columnAddress2}9+{columnAddress2}15)";
                //var formula6 = $"=SUM({columnAddress3}9+{columnAddress3}15)";

                //destSheetAll.Range[17, columnCount + 1].Formula = formula4;
                //destSheetAll.Range[17, columnCount + 2].Formula = formula5;
                //destSheetAll.Range[17, columnCount + 3].Formula = formula6;

            }
        }

        private void CreateSheetPerOperator()
        {
            foreach (var op in operators)
            {
                var data = new List<SpreadSheetObservation>();
                var obs = totalObs.Where(x => x.OperatorId == op.Id).ToList();
                var totalObsPerOperator = obs.Count;
                foreach (var observation in obs)
                {
                    var formattedDate = $"{observation.Date:d/M/yyyy HH:mm:ss}";
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
        }
    }
}

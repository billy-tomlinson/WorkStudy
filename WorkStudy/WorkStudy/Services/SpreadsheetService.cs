using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Syncfusion.XlsIO;
using WorkStudy.Model;
using Xamarin.Forms;

namespace WorkStudy.Services
{
    public class SpreadsheetService
    {
        private List<Operator> operators;
        private ActivitySampleStudy sample;
        private List<Activity> allStudyActivities;
        private List<Observation> totalObs;
        private List<List<ObservationSummary>> allTotals;

        private double timePerObservation;
        private double totalTimeMinutes;

        private IWorkbook workbook;
        private IWorksheet destSheetAll;

        private int ratedActivitiesTotalRowIndex;
        private int unRatedActivitiesTotalRowIndex;

        IStyle headerStyle;
        IStyle titleStyle;

        public SpreadSheet CreateExcelWorkBook()
        {
            var sampleRepo = new BaseRepository<ActivitySampleStudy>(Utilities.Connection);
            var activityRepo = new BaseRepository<Activity>(Utilities.Connection);
            var operatorRepo = new BaseRepository<Operator>(Utilities.Connection);
            var observationRepo = new BaseRepository<Observation>(Utilities.Connection);

            operators = operatorRepo.GetAllWithChildren().Where(cw => cw.StudyId == Utilities.StudyId).ToList();
            sample = sampleRepo.GetItem(Utilities.StudyId);
            allStudyActivities = activityRepo.GetItems().Where(x => x.StudyId == Utilities.StudyId).ToList();

            totalObs = observationRepo.GetItems().Where(x => x.StudyId == Utilities.StudyId).ToList();
            var totalCount = totalObs.Count();
            var firstOb = totalObs.Min(y => y.Date);
            var lastOb = totalObs.Max(y => y.Date);
            totalTimeMinutes = lastOb.Subtract(firstOb).TotalMinutes;
            timePerObservation = Math.Round(totalTimeMinutes / totalCount, 2);

            string path;
            string fileName = $"ActivitySample_StudyNo_{Utilities.StudyId}.xlsx";

            using (var excelEngine = new ExcelEngine())
            {
                //Set the default application version as Excel 2013.
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;

                //Create a workbook with a worksheet
                workbook = excelEngine.Excel.Workbooks.Create(1);

                headerStyle = workbook.Styles.Add("HeaderStyle");
                headerStyle.BeginUpdate();
                headerStyle.Color = Syncfusion.Drawing.Color.FromArgb(255, 174, 33);
                headerStyle.Font.Bold = true;
                headerStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                headerStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                headerStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                headerStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                headerStyle.EndUpdate();


                titleStyle = workbook.Styles.Add("TitleStyle");
                titleStyle.BeginUpdate();
                titleStyle.Color = Syncfusion.Drawing.Color.FromArgb(93, 173, 226);
                titleStyle.Font.Bold = true;
                titleStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                titleStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                titleStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                titleStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                titleStyle.EndUpdate();

                destSheetAll = workbook.Worksheets.Create("Summary");

                BuildRatedActivities();
                BuildUnRatedActivities();

                var stream = new MemoryStream();

                workbook.SaveAs(stream);
                workbook.Close();

                path = DependencyService.Get<ISave>()
                    .SaveSpreadSheet(fileName, "application/msexcel", stream)
                    .Result;
            }

            return new SpreadSheet() { FileName = fileName, FilePath = path };
        }

        private void BuildRatedActivities()
        {
            allTotals = new List<List<ObservationSummary>>();

            var allActivities = allStudyActivities.Where(x => x.Rated)
                .Select(y => new ActivityName() { Name = y.ActivityName.Name }).ToList();

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
                        destSheetAll.Range[1, columnCount + 2].CellStyle = headerStyle;

                        if (vv.ActivityName == v)
                        {
                            destSheetAll.Range[c, columnCount + 2].Number = vv.NumberOfObservations;
                            destSheetAll.Range[c, columnCount + 3].Number = vv.TotalTime;
                            destSheetAll.Range[c, columnCount + 4].Number = vv.Percentage;
                        }
                    }
                }

                var columnAddress1 =
                    Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]",
                        string.Empty);
                var columnAddress2 =
                    Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]",
                        string.Empty);
                var columnAddress3 =
                    Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 4].AddressLocal, @"[\d-]",
                        string.Empty);

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
                            var totalPerActivity = vv.TotalTime * totalActivity;

                            destSheetAll.Range[c, columnCount + 1].Number = Math.Round((double)totalActivity, 2);
                            destSheetAll.Range[c, columnCount + 2].Number = Math.Round((double)totalPerActivity, 2);
                            destSheetAll.Range[c, columnCount + 3].Number = Math.Round((double)totalPercent, 2);
                        }
                    }
                }

                var columnAddress1 =
                    Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 1].AddressLocal, @"[\d-]",
                        string.Empty);
                var columnAddress2 =
                    Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]",
                        string.Empty);
                var columnAddress3 =
                    Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]",
                        string.Empty);

                var formula1 = $"=SUM({columnAddress1}5:{columnAddress1}{allActivities.Count + 5})";
                var formula2 = $"=SUM({columnAddress2}5:{columnAddress2}{allActivities.Count + 5})";
                var formula3 = $"=SUM({columnAddress3}5:{columnAddress3}{allActivities.Count + 5})";

                destSheetAll.Range[allActivities.Count + 6, columnCount + 1].Formula = formula1;
                destSheetAll.Range[allActivities.Count + 6, columnCount + 2].Formula = formula2;
                destSheetAll.Range[allActivities.Count + 6, columnCount + 3].Formula = formula3;
            }

            destSheetAll.Range[allActivities.Count + 6, 1].Text = "SUB TOTAL EFFECTIVE";
            destSheetAll.Range[allActivities.Count + 6, 1, allActivities.Count + 6, columnCount + 3].CellStyle = headerStyle;
        }

        private void BuildUnRatedActivities()
        {
            allTotals = new List<List<ObservationSummary>>();

            var allActivities = allStudyActivities.Where(x => !x.Rated)
                .Select(y => new ActivityName() { Name = y.ActivityName.Name }).ToList();

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
                var columnAddress1 =
                    Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]",
                        string.Empty);
                var columnAddress2 =
                    Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]",
                        string.Empty);
                var columnAddress3 =
                    Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 4].AddressLocal, @"[\d-]",
                        string.Empty);

                var formula1 =
                    $"=SUM({columnAddress1}{unratedStartRow}:{columnAddress1}{allActivities.Count + unratedStartRow})";
                var formula2 =
                    $"=SUM({columnAddress2}{unratedStartRow}:{columnAddress2}{allActivities.Count + unratedStartRow})";
                var formula3 =
                    $"=SUM({columnAddress3}{unratedStartRow}:{columnAddress3}{allActivities.Count + unratedStartRow})";

                destSheetAll.Range[allActivities.Count + unratedStartRow + 1, columnCount + 2].Formula = formula1;
                destSheetAll.Range[allActivities.Count + unratedStartRow + 1, columnCount + 3].Formula = formula2;
                destSheetAll.Range[allActivities.Count + unratedStartRow + 1, columnCount + 4].Formula = formula3;

                unRatedActivitiesTotalRowIndex = allActivities.Count + unratedStartRow + 1;

                // Total All observations  - Add together total Rated +  total unrated
                var formula4 =
                    $"=SUM({columnAddress1}{ratedActivitiesTotalRowIndex}+{columnAddress1}{unRatedActivitiesTotalRowIndex})";
                var formula5 =
                    $"=SUM({columnAddress2}{ratedActivitiesTotalRowIndex}+{columnAddress2}{unRatedActivitiesTotalRowIndex})";
                var formula6 =
                    $"=SUM({columnAddress3}{ratedActivitiesTotalRowIndex}+{columnAddress3}{unRatedActivitiesTotalRowIndex})";

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
                            var totalPerActivity = vv.TotalTime * totalActivity;

                            destSheetAll.Range[c, columnCount + 1].Number = Math.Round((double)totalActivity, 2);
                            destSheetAll.Range[c, columnCount + 2].Number = Math.Round((double)totalPerActivity, 2);
                            destSheetAll.Range[c, columnCount + 3].Number = Math.Round((double)totalPercent, 2);
                        }
                    }
                }

                //total all unrated totals of all operators
                var columnAddress1 =
                    Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 1].AddressLocal, @"[\d-]",
                        string.Empty);
                var columnAddress2 =
                    Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]",
                        string.Empty);
                var columnAddress3 =
                    Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]",
                        string.Empty);

                var formula1 =
                    $"=SUM({columnAddress1}{unratedStartRow}:{columnAddress1}{allActivities.Count + unratedStartRow})";
                var formula2 =
                    $"=SUM({columnAddress2}{unratedStartRow}:{columnAddress2}{allActivities.Count + unratedStartRow})";
                var formula3 =
                    $"=SUM({columnAddress3}{unratedStartRow}:{columnAddress3}{allActivities.Count + unratedStartRow})";

                destSheetAll.Range[allActivities.Count + unratedStartRow + 1, columnCount + 1].Formula = formula1;
                destSheetAll.Range[allActivities.Count + unratedStartRow + 1, columnCount + 2].Formula = formula2;
                destSheetAll.Range[allActivities.Count + unratedStartRow + 1, columnCount + 3].Formula = formula3;

                destSheetAll.Range[allActivities.Count + unratedStartRow + 1, 1].Text = "SUB TOTAL INEFFECTIVE";
                destSheetAll.Range[allActivities.Count + unratedStartRow + 1, 1, allActivities.Count + unratedStartRow + 1, columnCount + 3].CellStyle = headerStyle;

                // Total All observations  - Add together total Rated +  total unrated
                var formula4 =
                    $"=SUM({columnAddress1}{ratedActivitiesTotalRowIndex}+{columnAddress1}{unRatedActivitiesTotalRowIndex})";
                var formula5 =
                    $"=SUM({columnAddress2}{ratedActivitiesTotalRowIndex}+{columnAddress2}{unRatedActivitiesTotalRowIndex})";
                var formula6 =
                    $"=SUM({columnAddress3}{ratedActivitiesTotalRowIndex}+{columnAddress3}{unRatedActivitiesTotalRowIndex})";

                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 1].Formula = formula4;
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 2].Formula = formula5;
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 3].Formula = formula6;

                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, 1].Text = "TOTAL";
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, 1, unRatedActivitiesTotalRowIndex + 2, columnCount + 3].CellStyle = headerStyle;

                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 1].CellStyle = titleStyle;
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 2].CellStyle = titleStyle;
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 3].CellStyle = titleStyle;

                destSheetAll.Range[3, 1, 3, columnCount + 3].CellStyle = titleStyle;

                destSheetAll.Range[1, 1, unRatedActivitiesTotalRowIndex + 2, columnCount + 3].AutofitColumns();
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
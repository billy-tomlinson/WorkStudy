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

        int startRowIndex;
        int valueAddedActivitiesTotalRowIndex;
        int nonValueAddedActivitiesTotalRowIndex;
        int unRatedActivitiesTotalRowIndex;

        IStyle headerStyle;
        IStyle titleStyle;
        IStyle totalsStyle;
        IStyle detailsStyle;

        public SpreadSheet CreateExcelWorkBook()
        {
            var sampleRepo = new BaseRepository<ActivitySampleStudy>(Utilities.Connection);
            var activityRepo = new BaseRepository<Activity>(Utilities.Connection);
            var operatorRepo = new BaseRepository<Operator>(Utilities.Connection);
            var observationRepo = new BaseRepository<Observation>(Utilities.Connection);

            operators = operatorRepo.GetAllWithChildren().Where(cw => cw.StudyId == Utilities.StudyId).ToList();
            sample = sampleRepo.GetItem(Utilities.StudyId);
            allStudyActivities = activityRepo.GetAllWithChildren().Where(x => x.StudyId == Utilities.StudyId).ToList();

            totalObs = observationRepo.GetItems().Where(x => x.StudyId == Utilities.StudyId).ToList();
            var totalCount = totalObs.Count();
            var firstOb = totalObs.Min(y => y.Date);
            var lastOb = totalObs.Max(y => y.Date);
            totalTimeMinutes = lastOb.Subtract(firstOb).TotalMinutes;
            timePerObservation = Math.Round(totalTimeMinutes / totalCount, 2);

            string path;
            string fileName = $"ActivitySample_Study_{Utilities.StudyId}.xlsx";

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
                headerStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                headerStyle.EndUpdate();


                titleStyle = workbook.Styles.Add("TitleStyle");
                titleStyle.BeginUpdate();
                titleStyle.Color = Syncfusion.Drawing.Color.FromArgb(93, 173, 226);
                titleStyle.Font.Bold = true;
                titleStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                titleStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                titleStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                titleStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                titleStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                titleStyle.EndUpdate();

                totalsStyle = workbook.Styles.Add("TotalsStyle");
                totalsStyle.BeginUpdate();
                totalsStyle.Color = Syncfusion.Drawing.Color.FromArgb(255, 255, 153);
                totalsStyle.Font.Bold = true;
                totalsStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                totalsStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                totalsStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                totalsStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                totalsStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                totalsStyle.EndUpdate();

                detailsStyle = workbook.Styles.Add("DetailsStyle");
                detailsStyle.BeginUpdate();
                detailsStyle.Color = Syncfusion.Drawing.Color.FromArgb(255, 255, 153);
                detailsStyle.Font.Bold = true;
                detailsStyle.Font.Size = 20;
                detailsStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                detailsStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                detailsStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                detailsStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                detailsStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                detailsStyle.EndUpdate();

                BuildStudyDetails();

                destSheetAll = workbook.Worksheets.Create("Summary");

                BuildValueAddedRatedActivities();
                BuildNonValueAddedRatedActivities();
                BuildUnRatedActivities();

                workbook.Worksheets[0].Remove();

                var stream = new MemoryStream();

                workbook.SaveAs(stream);
                workbook.Close();

                path = DependencyService.Get<ISave>()
                    .SaveSpreadSheet(fileName, "application/msexcel", stream)
                    .Result;
            }

            return new SpreadSheet() { FileName = fileName, FilePath = path };
        }

        private void BuildStudyDetails()
        {
            var destSheetStudyDetails = workbook.Worksheets.Create("Study Details");

            destSheetStudyDetails.Range["A2"].CellStyle = detailsStyle;
            destSheetStudyDetails.Range["A3"].CellStyle = detailsStyle;
            destSheetStudyDetails.Range["A4"].CellStyle = detailsStyle;
            destSheetStudyDetails.Range["A5"].CellStyle = detailsStyle;
            destSheetStudyDetails.Range["A6"].CellStyle = detailsStyle;
            destSheetStudyDetails.Range["A7"].CellStyle = detailsStyle;
            destSheetStudyDetails.Range["A8"].CellStyle = detailsStyle;

            destSheetStudyDetails.Range["A2"].Text = "Study Number";
            destSheetStudyDetails.Range["A3"].Text = "Name";
            destSheetStudyDetails.Range["A4"].Text = "Department";
            destSheetStudyDetails.Range["A5"].Text = "Studied By";
            destSheetStudyDetails.Range["A6"].Text = "Date";
            destSheetStudyDetails.Range["A7"].Text = "Time";
            destSheetStudyDetails.Range["A8"].Text = "Rated";


            destSheetStudyDetails.Range["B2"].CellStyle = detailsStyle;
            destSheetStudyDetails.Range["B3"].CellStyle = detailsStyle;
            destSheetStudyDetails.Range["B4"].CellStyle = detailsStyle;
            destSheetStudyDetails.Range["B5"].CellStyle = detailsStyle;
            destSheetStudyDetails.Range["B6"].CellStyle = detailsStyle;
            destSheetStudyDetails.Range["B7"].CellStyle = detailsStyle;
            destSheetStudyDetails.Range["B8"].CellStyle = detailsStyle;

            destSheetStudyDetails.Range["B2"].Text = sample.StudyNumber.ToString();
            destSheetStudyDetails.Range["B3"].Text = sample.Name;
            destSheetStudyDetails.Range["B4"].Text = sample.Department;
            destSheetStudyDetails.Range["B5"].Text = sample.StudiedBy;
            destSheetStudyDetails.Range["B6"].Text = sample.DateFormatted;
            destSheetStudyDetails.Range["B7"].Text = sample.TimeFormatted;
            destSheetStudyDetails.Range["B8"].Text = sample.IsRated.ToString();

            destSheetStudyDetails.Range[1, 1, 8, 2].AutofitColumns();
        }

        private void BuildValueAddedRatedActivities()
        {
            startRowIndex = 5;

            allTotals = new List<List<ObservationSummary>>();

            var allActivities = allStudyActivities.Where(x => x.Rated && x.IsValueAdded)
                .Select(y => new { y.ActivityName.Name }).ToList();

            destSheetAll.Range[3, 1].Text = "Activity";
            destSheetAll.ImportData(allActivities, startRowIndex, 1, false);

            CreateSheetPerOperator();

            var columnCount = 1;

            var computedRange = $"A{startRowIndex}:A{allActivities.Count + startRowIndex}";
            var range = destSheetAll[computedRange].ToList();

            foreach (var item in allTotals)
            {
                destSheetAll.Range[3, columnCount + 2].Text = "Obs Req";
                destSheetAll.Range[3, columnCount + 3].Text = "Total Obs";
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
                            var columnAddress = destSheetAll.Range[c, columnCount + 4].AddressLocal;
                            var formula = $"=SUM(4*{columnAddress})*(100-{columnAddress})/100";

                            destSheetAll.Range[c, columnCount + 2].NumberFormat = "###0";
                            destSheetAll.Range[c, columnCount + 2].Formula = formula;
                            destSheetAll.Range[c, columnCount + 3].Number = vv.NumberOfObservations;
                            destSheetAll.Range[c, columnCount + 4].Number = vv.Percentage;
                        }
                    }
                }

                var columnAddress1 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress2 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress3 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 4].AddressLocal, @"[\d-]", string.Empty);

                var formula2 = $"=SUM({columnAddress2}{startRowIndex}:{columnAddress2}{allActivities.Count + startRowIndex})";
                var formula3 = $"=SUM({columnAddress3}{startRowIndex}:{columnAddress3}{allActivities.Count + startRowIndex})";

                destSheetAll.Range[allActivities.Count + 6, columnCount + 3].Formula = formula2;
                destSheetAll.Range[allActivities.Count + 6, columnCount + 4].Formula = formula3;

                valueAddedActivitiesTotalRowIndex = allActivities.Count + 6;

                columnCount = columnCount + 4;
            }

            destSheetAll.Range[3, columnCount + 2].Text = "OBS REQ";
            destSheetAll.Range[3, columnCount + 3].Text = "OBS TOTAL";
            destSheetAll.Range[3, columnCount + 4].Text = "% TOTAL";


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
                            var columnAddress = destSheetAll.Range[c, columnCount + 4].AddressLocal;
                            var formula = $"=SUM(4*{columnAddress})*(100-{columnAddress})/100";

                            var totalActivity = totalObs.Count(x => x.ActivityName == v);
                            var totalObsCount = totalObs.Count();
                            var totalPercent = Math.Round((double)totalActivity / totalObsCount * 100, 2);

                            destSheetAll.Range[c, columnCount + 2].NumberFormat = "###0";
                            destSheetAll.Range[c, columnCount + 2].Formula = formula;
                            destSheetAll.Range[c, columnCount + 3].Number = Math.Round((double)totalActivity, 2);
                            destSheetAll.Range[c, columnCount + 4].Number = Math.Round((double)totalPercent, 2);
                        }
                    }
                }

                var columnAddress1 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress2 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress3 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 4].AddressLocal, @"[\d-]", string.Empty);

                var formula2 = $"=SUM({columnAddress2}{startRowIndex}:{columnAddress2}{allActivities.Count + startRowIndex})";
                var formula3 = $"=SUM({columnAddress3}{startRowIndex}:{columnAddress3}{allActivities.Count + startRowIndex})";

                destSheetAll.Range[allActivities.Count + 6, columnCount + 3].Formula = formula2;
                destSheetAll.Range[allActivities.Count + 6, columnCount + 4].Formula = formula3;

            }

            destSheetAll.Range[allActivities.Count + 6, 1].Text = "SUB TOTAL VALUE ADDED";
            destSheetAll.Range[allActivities.Count + 6, 1, allActivities.Count + 6, columnCount + 4].CellStyle = headerStyle;
        }

        private void BuildNonValueAddedRatedActivities()
        {
            allTotals = new List<List<ObservationSummary>>();

            var allActivities = allStudyActivities.Where(x => x.Rated && !x.IsValueAdded)
             .Select(y => new { y.ActivityName.Name }).ToList();

            var startRow = valueAddedActivitiesTotalRowIndex + 2;

            destSheetAll.ImportData(allActivities, startRow, 1, false);

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

            var computedRange = $"A{startRow}:A{startRow + allActivities.Count}";
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
                            var columnAddress = destSheetAll.Range[c, columnCount + 4].AddressLocal;
                            var formula = $"=SUM(4*{columnAddress})*(100-{columnAddress})/100";

                            destSheetAll.Range[c, columnCount + 2].NumberFormat = "###0";
                            destSheetAll.Range[c, columnCount + 2].Formula = formula;
                            destSheetAll.Range[c, columnCount + 3].Number = vv.NumberOfObservations;
                            destSheetAll.Range[c, columnCount + 4].Number = vv.Percentage;
                        }
                    }
                }

                // Total All Unrated observations
                var columnAddress1 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress2 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress3 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 4].AddressLocal, @"[\d-]", string.Empty);

                var formula2 = $"=SUM({columnAddress2}{startRow}:{columnAddress2}{allActivities.Count + startRow})";
                var formula3 = $"=SUM({columnAddress3}{startRow}:{columnAddress3}{allActivities.Count + startRow})";

                destSheetAll.Range[allActivities.Count + startRow + 1, columnCount + 3].Formula = formula2;
                destSheetAll.Range[allActivities.Count + startRow + 1, columnCount + 4].Formula = formula3;

                nonValueAddedActivitiesTotalRowIndex = allActivities.Count + startRow + 1;

                columnCount = columnCount + 4;
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
                            var columnAddress = destSheetAll.Range[c, columnCount + 4].AddressLocal;
                            var formula = $"=SUM(4*{columnAddress})*(100-{columnAddress})/100";

                            var totalActivity = totalObs.Count(x => x.ActivityName == v);
                            var totalObsCount = totalObs.Count();
                            var totalPercent = Math.Round((double)totalActivity / totalObsCount * 100, 2);

                            destSheetAll.Range[c, columnCount + 2].NumberFormat = "###0";
                            destSheetAll.Range[c, columnCount + 2].Formula = formula;
                            destSheetAll.Range[c, columnCount + 3].Number = Math.Round((double)totalActivity, 2);
                            destSheetAll.Range[c, columnCount + 4].Number = Math.Round((double)totalPercent, 2);
                        }
                    }
                }

                //total all unrated totals of all operators
                var columnAddress2 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress3 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 4].AddressLocal, @"[\d-]", string.Empty);

                var formula2 = $"=SUM({columnAddress2}{startRow}:{columnAddress2}{allActivities.Count + startRow})";
                var formula3 = $"=SUM({columnAddress3}{startRow}:{columnAddress3}{allActivities.Count + startRow})";

                destSheetAll.Range[allActivities.Count + startRow + 1, columnCount + 3].Formula = formula2;
                destSheetAll.Range[allActivities.Count + startRow + 1, columnCount + 4].Formula = formula3;

                destSheetAll.Range[allActivities.Count + startRow + 1, 1].Text = "SUB TOTAL NON VALUE ADDED";
                destSheetAll.Range[allActivities.Count + startRow + 1, 1, allActivities.Count + startRow + 1, columnCount + 4].CellStyle = headerStyle;
            }

            destSheetAll.Range[allActivities.Count + startRow + 1, 1].Text = "SUB TOTAL NON VALUE ADDED";
            destSheetAll.Range[allActivities.Count + startRow + 1, 1, allActivities.Count + startRow + 1, columnCount + 4].CellStyle = headerStyle;
        }

        private void BuildUnRatedActivities()
        {
            allTotals = new List<List<ObservationSummary>>();

            var allActivities = allStudyActivities.Where(x => !x.Rated)
             .Select(y => new { y.ActivityName.Name }).ToList();

            var startRow = nonValueAddedActivitiesTotalRowIndex + 2;

            destSheetAll.ImportData(allActivities, startRow, 1, false);

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

            var computedRange = $"A{startRow}:A{startRow + allActivities.Count}";
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

                            var columnAddress = destSheetAll.Range[c, columnCount + 4].AddressLocal;
                            var formula = $"=SUM(4*{columnAddress})*(100-{columnAddress})/100";

                            destSheetAll.Range[c, columnCount + 2].NumberFormat = "###0";
                            destSheetAll.Range[c, columnCount + 2].Formula = formula;
                            destSheetAll.Range[c, columnCount + 3].Number = vv.NumberOfObservations;
                            destSheetAll.Range[c, columnCount + 4].Number = vv.Percentage;
                        }
                    }
                }

                // Total All Unrated observations
                var columnAddress1 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress2 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress3 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 4].AddressLocal, @"[\d-]", string.Empty);

                var formula2 = $"=SUM({columnAddress2}{startRow}:{columnAddress2}{allActivities.Count + startRow})";
                var formula3 = $"=SUM({columnAddress3}{startRow}:{columnAddress3}{allActivities.Count + startRow})";

                destSheetAll.Range[allActivities.Count + startRow + 1, columnCount + 3].Formula = formula2;
                destSheetAll.Range[allActivities.Count + startRow + 1, columnCount + 4].Formula = formula3;

                unRatedActivitiesTotalRowIndex = allActivities.Count + startRow + 1;

                // Total All observations  - Add together total value added +  total value added +  total unrated
                var formula4 = $"=TEXT(SUM({columnAddress1}{startRowIndex}:{columnAddress1}{unRatedActivitiesTotalRowIndex}), \"####\")";
                var formula5 = $"=SUM({columnAddress2}{valueAddedActivitiesTotalRowIndex}+{columnAddress2}{nonValueAddedActivitiesTotalRowIndex}+{columnAddress2}{unRatedActivitiesTotalRowIndex})";
                var formula6 = $"=TEXT(SUM({columnAddress3}{valueAddedActivitiesTotalRowIndex}+{columnAddress3}{nonValueAddedActivitiesTotalRowIndex}+{columnAddress3}{unRatedActivitiesTotalRowIndex}), \"00.0\")";

                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 2].Formula = formula4;
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 3].NumberFormat = "###0";
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 3].Formula = formula5;
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 4].Formula = formula6;

                columnCount = columnCount + 4;
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

                            var columnAddress = destSheetAll.Range[c, columnCount + 4].AddressLocal;
                            var formula = $"=SUM(4*{columnAddress})*(100-{columnAddress})/100";

                            var totalActivity = totalObs.Count(x => x.ActivityName == v);
                            var totalObsCount = totalObs.Count();
                            var totalPercent = Math.Round((double)totalActivity / totalObsCount * 100, 2);
                            var totalPerActivity = vv.TotalTime * totalActivity;

                            destSheetAll.Range[c, columnCount + 2].NumberFormat = "###0";
                            destSheetAll.Range[c, columnCount + 2].Formula = formula;
                            destSheetAll.Range[c, columnCount + 3].Number = Math.Round((double)totalActivity, 2);
                            destSheetAll.Range[c, columnCount + 4].Number = Math.Round((double)totalPercent, 2);
                        }
                    }
                }

                //total all unrated totals of all operators
                var columnAddress1 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 2].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress2 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 3].AddressLocal, @"[\d-]", string.Empty);
                var columnAddress3 = Regex.Replace(destSheetAll.Range[allActivities.Count + 6, columnCount + 4].AddressLocal, @"[\d-]", string.Empty);

                var formula1 = $"=SUM({columnAddress1}{startRow}:{columnAddress1}{allActivities.Count + startRow})";
                var formula2 = $"=SUM({columnAddress2}{startRow}:{columnAddress2}{allActivities.Count + startRow})";
                var formula3 = $"=SUM({columnAddress3}{startRow}:{columnAddress3}{allActivities.Count + startRow})";

                destSheetAll.Range[allActivities.Count + startRow + 1, columnCount + 3].Formula = formula2;
                destSheetAll.Range[allActivities.Count + startRow + 1, columnCount + 4].Formula = formula3;

                destSheetAll.Range[allActivities.Count + startRow + 1, 1].Text = "SUB TOTAL INEFFECTIVE";
                destSheetAll.Range[allActivities.Count + startRow + 1, 1, allActivities.Count + startRow + 1, columnCount + 4].CellStyle = headerStyle;


                // Total All observations  - Add together total value added +  total value added +  total unrated
                var formula4 = $"=TEXT(SUM({columnAddress1}{startRowIndex}:{columnAddress1}{unRatedActivitiesTotalRowIndex}), \"####\")";
                var formula5 = $"=SUM({columnAddress2}{valueAddedActivitiesTotalRowIndex}+{columnAddress2}{nonValueAddedActivitiesTotalRowIndex}+{columnAddress2}{unRatedActivitiesTotalRowIndex})";
                var formula6 = $"=TEXT(SUM({columnAddress3}{valueAddedActivitiesTotalRowIndex}+{columnAddress3}{nonValueAddedActivitiesTotalRowIndex}+{columnAddress3}{unRatedActivitiesTotalRowIndex}), \"00.0\")";

                //**** THIS TOTALS ALL THE TOTALS AT THE END OF THE SHEET *********************************
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 2].Formula = formula4;
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 3].NumberFormat = "###0";
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 3].Formula = formula5;
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 4].Formula = formula6;
                //******************************************************************************************


                //**** THIS STYLES THE TOTALS *********************************
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, 1].Text = "TOTAL";
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, 1, unRatedActivitiesTotalRowIndex + 2, columnCount + 4].CellStyle = headerStyle;

                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 2].CellStyle = titleStyle;
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 3].CellStyle = titleStyle;
                destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 4].CellStyle = titleStyle;
                destSheetAll.Range[3, 1, 3, columnCount + 4].CellStyle = titleStyle;


                destSheetAll.Range[1, 1, unRatedActivitiesTotalRowIndex + 2, columnCount + 4].AutofitColumns();
                //******************************************************************************************
            }

            destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, 1].Text = "TOTAL";
            destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, 1, unRatedActivitiesTotalRowIndex + 2, columnCount + 4].CellStyle = headerStyle;

            destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 2].CellStyle = totalsStyle;
            destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 3].CellStyle = totalsStyle;
            destSheetAll.Range[unRatedActivitiesTotalRowIndex + 2, columnCount + 4].CellStyle = totalsStyle;

            destSheetAll.Range[3, columnCount + 2].CellStyle = totalsStyle;
            destSheetAll.Range[3, columnCount + 3].CellStyle = totalsStyle;
            destSheetAll.Range[3, columnCount + 4].CellStyle = totalsStyle;
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
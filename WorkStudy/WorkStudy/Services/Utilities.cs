﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Messaging;
using Syncfusion.XlsIO;
using WorkStudy.Model;
using Xamarin.Forms;

namespace WorkStudy.Services
{
    public class Utilities
    {
        public static int StudyId { get; set; }
        public static bool IsCompleted { get; set; }
        public static bool RatedStudy { get; set; }
        public static bool AllObservationsTaken { get; set; }
        public static string Connection { get; set; }
        public static DateTime LastNotification { get; set; } = DateTime.Now;

        public static async void Navigate(Page page)
        {
            await Task.Delay(500);
            await App.NavigationPage.Navigation.PushAsync(page);
        }

        public static Color Clicked = Color.Orange;
        public static Color UnClicked = Color.FromHex("#4174f4");

        public static string ValueAddedColour = "#D5F0F1";
        public static string NonValueAddedColour = "#FAFDBC";
        public static string ValidColour = "#d5f0f1";


        public static string DeleteImage = "delete.png";
        public static string UndoImage = "undo.png";
        public static string CommentsImage = "comments.png";
        public static bool   CancelAlarm { get; set; }

        public static bool ContinueTimer { get; set; } = true;

        public static void ClearNavigation()
        {

            var existingPages = App.NavigationPage?.Navigation.NavigationStack.ToList();

            for (int i = 0; i < existingPages?.Count; i++)
            {
                if (i != existingPages.Count - 1)
                    App.NavigationPage.Navigation.RemovePage(existingPages[i]);
            }
        }

        public static ObservableCollection<MultipleActivities> BuildGroupOfActivities(ObservableCollection<Activity> activites)
        {
            int counter = 0;
            bool added = false;
            var multipleActivities = new MultipleActivities();
            var groupedActivities = new ObservableCollection<MultipleActivities>();

            for (int i = 0; i < activites.Count; i++)
            {
                var activity = activites[i];

                if (counter == 0)
                {
                    multipleActivities.ActivityOne = activity;
                    added = false;
                    counter++;
                }

                else if (counter == 1)
                {
                    multipleActivities.ActivityTwo = activity;
                    added = false;
                    counter++;
                }

                else if (counter == 2)
                {
                    multipleActivities.ActivityThree = activity;
                    groupedActivities.Add(multipleActivities);
                    added = true;
                    multipleActivities = new MultipleActivities();
                    counter = 0;
                }
            }

            if (!added) groupedActivities.Add(multipleActivities);

            return groupedActivities;
        }

        public static SpreadSheet CreateExcelWorkBook<T>(IEnumerable<T> items)
        {
            string path;
            string fileName = $"Workstudy_Study_{StudyId}.xlsx";


            var obsRepo = new BaseRepository<Observation>(Connection);
            var opsRepo = new BaseRepository<Operator>(Connection);

            var operators = opsRepo.GetAllWithChildren().Where(x => x.StudyId == StudyId);
                                   
            using (var excelEngine = new ExcelEngine())
            {
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;

                var workbook = excelEngine.Excel.Workbooks.Create(1);

                foreach (var op in operators)
                {
                    var data = new List<SpreadSheetObservation>();
                    var obs = obsRepo.GetItems().Where(x => x.OperatorId == op.Id);

                    foreach (var observation in obs)
                    {
                        data.Add(new SpreadSheetObservation()
                        {
                            ActivityName = observation.ActivityName,
                           // StudyId = StudyId,
                            OperatorName = op.Name,
                            ObservationNumber = observation.ObservationNumber,
                            Rating = observation.Rating

                        });
                    }
                    var destSheet = workbook.Worksheets.Create(op.Name);
                    destSheet.ImportData(data, 1, 1, true);

                }

                MemoryStream stream = new MemoryStream();

                workbook.SaveAs(stream);
                workbook.Close();

                path = DependencyService.Get<ISave>()
                                        .SaveSpreadSheet(fileName, "application/msexcel", stream)
                                        .Result;
            }

            return new SpreadSheet() { FileName = fileName, FilePath = path };
        }

        public static bool SendEmail(SpreadSheet spreadSheet)
        {
            try
            {
                var email = new EmailMessageBuilder()
                    .Subject("Activity Sample Results")
                    .Body($"Attached are the results for Study {StudyId}")
                    .WithAttachment(Path.Combine(spreadSheet.FilePath, spreadSheet.FileName), "application/msexcel")
                    .Build();

                var emailTask = CrossMessaging.Current.EmailMessenger;
                if (emailTask.CanSendEmail)
                {
                    emailTask.SendEmail(email);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static int CalculateObservationsRequired(double activityPercentage)
        {

            var percentage = activityPercentage == 100 ? 1 : activityPercentage;
            //n=4p - n = ( 4 x 17 ) = 68
            var fourMultipliedByPercentage = 4 * percentage;

            //(100-p) = (100 - 17 ) = 83
            var oneHundredMinusPercentage = 100 - percentage;

            //n=4p(100-p) = 68 x 83 = 5644
            var calculationOfPercentage = fourMultipliedByPercentage * oneHundredMinusPercentage;

            //n=4p(100-p)/L x L  = 5644/100
            var numberOfObservations = (int)calculationOfPercentage / 100;

            return numberOfObservations;

        }
    }
}

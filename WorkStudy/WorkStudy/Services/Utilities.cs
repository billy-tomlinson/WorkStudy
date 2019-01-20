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


        public static bool OperatorTableUpdated { get; set; }
        public static bool ActivityTableUpdated { get; set; }
        public static bool ObservationTableUpdated { get; set; }

        public static bool MainPageHasUpdatedActivityChanges { get; set; }
        public static bool MainPageHasUpdatedOperatorChanges { get; set; }
        public static bool MainPageHasUpdatedObservationChanges { get; set; }

        public static bool ActivityPageHasUpdatedActivityChanges { get; set; }
        public static bool ActivityPageHasUpdatedOperatorChanges { get; set; }
        public static bool ActivityPageHasUpdatedObservationChanges { get; set; }

        public static bool OperatorPageHasUpdatedActivityChanges { get; set; }
        public static bool OperatorPageHasUpdatedOperatorChanges { get; set; }

        public static bool MergePageHasUpdatedActivityChanges { get; set; }

        public static bool AllActivitiesPageHasUpdatedActivityChanges { get; set; }

        public static IBaseRepository<AlarmDetails> AlarmRepo = 
            new BaseRepository<AlarmDetails>(Connection);

        static bool restartAlarmCounter;
        public static bool RestartAlarmCounter
        {
            get => restartAlarmCounter;
            set
            {
                restartAlarmCounter = value;
                UpdateAlarm();
            }
        }
        public static void CheckIfAlarmHasExpiredWhilstInBackgroundMode()
        {
            UpdateAlarmAfterBeingInBackround();

        }
        private static void UpdateAlarm()
        {
            if(restartAlarmCounter && StudyId > 0)
            {
                SaveNewAlarmDetails();
            }
        }

        private static void UpdateAlarmAfterBeingInBackround()
        {
            if (StudyId > 0)
            {
                var alarm = AlarmRepo.GetItems().SingleOrDefault(x => x.StudyId == StudyId);
                bool notificationExpired = alarm.NotificationRecieved < DateTime.Now;
                if(notificationExpired)
                    SaveNewAlarmDetails();
            }
        }

        private static void SaveNewAlarmDetails()
        {
            var alarm = AlarmRepo.GetItems().SingleOrDefault(x => x.StudyId == StudyId);
            if (alarm.Type != "RANDOM")
                alarm.NotificationRecieved = DateTime.Now.AddSeconds(alarm.Interval);
            else
            {
                Random r = new Random();
                var intervalTime = r.Next(0, alarm.Interval * 2);
                var nextObsTime = intervalTime < 60 ? 61 : intervalTime;
                alarm.NotificationRecieved = DateTime.Now.AddSeconds(nextObsTime);
                SetNextRandomAlarmTime(nextObsTime);
            }
            AlarmRepo.SaveItem(alarm);
        }

        private static void  SetNextRandomAlarmTime(int nextAlarm)
        {
            var service = DependencyService.Get<ILocalNotificationService>();

            service.DisableLocalNotification("Alert", "Next Observation Round", 0, DateTime.Now);
            service.LocalNotification("Alert", "Next Observation Round", 0, DateTime.Now, nextAlarm);
        }

        public static void UpdateTableFlags()
        {
            if (MainPageHasUpdatedActivityChanges && ActivityPageHasUpdatedActivityChanges 
                    && OperatorPageHasUpdatedActivityChanges && MergePageHasUpdatedActivityChanges
                    && AllActivitiesPageHasUpdatedActivityChanges)
                ActivityTableUpdated = false;

            if (MainPageHasUpdatedOperatorChanges && ActivityPageHasUpdatedOperatorChanges
                    && OperatorPageHasUpdatedOperatorChanges)
                OperatorTableUpdated = false;

            if (MainPageHasUpdatedObservationChanges && ActivityPageHasUpdatedObservationChanges)
                ObservationTableUpdated = false;
        }

        public static async Task Navigate(Page page)
        {
            await Task.Delay(500);
            await App.NavigationPage.Navigation.PushAsync(page);
        }

        public static Color Clicked = Color.FromHex("#CCCCCE");
        public static Color UnClicked = Color.FromHex("#4174f4");

        public const string ValueAddedColour = "#61FF4B";
        public const string NonValueAddedColour = "#FFE14B";
        public const string InactiveColour = "#FF644B";
        public const string ValidColour = "#d5f0f1";
        public const string ClickedHex = "#CCCCCE";


        public const string DeleteImage = "delete.png";
        public static string UndoImage = "undo.png";
        public const string CommentsImage = "comments.png";
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

        public static void SendEmail(SpreadSheet spreadSheet)
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

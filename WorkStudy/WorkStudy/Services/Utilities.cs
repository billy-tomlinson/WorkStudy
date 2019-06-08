using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Messaging;
using Syncfusion.XlsIO;
using TimeStudy.Pages;
using WorkStudy.Model;
using WorkStudy.Pages;
using Xamarin.Forms;

namespace WorkStudy.Services
{
    public class Utilities
    {
        public static int StudyId { get; set; }
        public static bool IsCompleted { get; set; }
        public static bool RatedStudy { get; set; }
        public static bool AllObservationsTaken { get; set; }
        public static bool SetUpForNextObservationRound { get; set; }
        public static bool CloseRunningTotals { get; set; }
        public static string Version { get => DependencyService.Get<IAppVersion>().GetVersion();}
        public static string Build { get => DependencyService.Get<IAppVersion>().GetBuild(); }

        public static ObservableCollection<OperatorObservation> OperatorObservations { get; set; }

        public static string Connection { get; set; }
        public static string AlarmConnection { get; set; }
        public static DateTime LastNotification { get; set; } = DateTime.Now;

        public static bool OperatorTableUpdated { get; set; }
        public static bool ActivityTableUpdated { get; set; }
        public static bool ObservationTableUpdated { get; set; }
        public static bool ActivitySampleTableUpdated { get; set; }

        public static bool MainPageHasUpdatedActivityChanges { get; set; }
        public static bool MainPageHasUpdatedOperatorChanges { get; set; }
        public static bool MainPageHasUpdatedObservationChanges { get; set; }

        public static bool ActivityPageHasUpdatedActivityChanges { get; set; }
        public static bool ActivityPageHasUpdatedOperatorChanges { get; set; }
        public static bool ActivityPageHasUpdatedObservationChanges { get; set; }

        public static bool OperatorPageHasUpdatedActivityChanges { get; set; }
        public static bool OperatorPageHasUpdatedOperatorChanges { get; set; }
        public static bool OperatorPageHasUpdatedActivitySampleChanges { get; set; }

        public static bool MergePageHasUpdatedActivityChanges { get; set; }

        public static bool AllActivitiesPageHasUpdatedActivityChanges { get; set; }
        public static bool LapButtonClicked { get; set; }

        public static IBaseRepository<AlarmDetails> AlarmRepo =
            new BaseRepository<AlarmDetails>(AlarmConnection);

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

            if (OperatorPageHasUpdatedActivitySampleChanges)
                ActivitySampleTableUpdated = false;
        }

        public static async Task Navigate(Page page)
        {
            await App.NavigationPage.Navigation.PushAsync(page);
        }

        public static Color Clicked = Color.FromHex("#CCCCCE");
        public static Color UnClicked = Color.FromHex("#4174f4");

        public const string ValueAddedColour = "#74D3AE";
        public const string NonValueAddedColour = "#E8D671";
        public const string InactiveColour = "#ED6A5A";
        public const string ValidColour = "#d5f0f1";
        public const string ClickedHex = "#CCCCCE";

        public const string RASBackGroundColour = "#75A2B5";
        public const string RAStudyTabBarBackgroundColour = "#75A2B5";

        public const string StopWatchBackGroundColour = "#476C9B";

        public const string DeleteImage = "delete.png";
        public static string UndoImage = "undo.png";
        public const string CommentsImage = "comments.png";
        public static bool CancelAlarm { get; set; }

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
            Guid guid = Guid.NewGuid();
            string fileName = $"Workstudy_{guid}_{StudyId}.xlsx";


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

        public static void MoveObservationsToHistoryTable()
        {
            var observationsHistoricRepo = new BaseRepository<ObservationHistoric>(Connection);
            var observationsRepo = new BaseRepository<Observation>(Connection);

            var sqlCommand = "INSERT INTO ObservationHistoric SELECT o.* FROM Observation o INNER JOIN ActivitySampleStudy ass ON o.StudyId = ass.ID WHERE ass.Completed = 1";
            observationsHistoricRepo.ExecuteSQLCommand(sqlCommand);
            sqlCommand = "DELETE FROM Observation WHERE StudyId in (SELECT Id from ActivitySampleStudy WHERE Completed = 1)";
            observationsRepo.ExecuteSQLCommand(sqlCommand);
        }

        public static void SwitchTimeStudyMenuEvent()
        {
            var menuPage = new HamburgerMenuPage() { Title = "Main Page", Icon = "hamburger.png" };

            var md = (MasterDetailPage)Application.Current.MainPage;
            md.Master = menuPage;
            var navPage = md.Detail as NavigationPage;
            navPage.BarBackgroundColor = Color.FromHex(TimeStudy.Services.Utilities.TimeStudyTabBarBackgroundColour);
        }

        public static void SwitchRASMenuEvent()
        {
            var menuPage = new MenuPage() { Title = "Main Page", Icon = "hamburger.png" };

            var md = (MasterDetailPage)Application.Current.MainPage;
            md.Master = menuPage;
            var navPage = md.Detail as NavigationPage;
            navPage.BarBackgroundColor = Color.FromHex(RASBackGroundColour);
        }

        public static void SwitchStopWatchMenuEvent()
        {
            var menuPage = new StopWatch.StopWatchMenuPage() { Title = "Main Page", Icon = "hamburger.png" };

            var md = (MasterDetailPage)Application.Current.MainPage;
            md.Master = menuPage;
            var navPage = md.Detail as NavigationPage;
            navPage.BarBackgroundColor = Color.FromHex(StopWatchBackGroundColour);
        }

        public static void SwitchHomeMenuEvent()
        {
            var menuPage = new HomeMenuPage() { Title = "Main Page", Icon = "hamburger.png" };

            var md = (MasterDetailPage)Application.Current.MainPage;
            md.Master = menuPage;
            var navPage = md.Detail as NavigationPage;
            navPage.BarBackgroundColor = Color.FromHex(RASBackGroundColour);
        }
    }

}
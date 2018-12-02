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
        public static int OperatorId { get; set; }
        public static int StudySetUpComplete { get; set; }
        public static bool AllObservationsTaken { get; set; }

        public static async void Navigate(ContentPage page)
        {
            await Task.Delay(1000);
            await App.NavigationPage.Navigation.PushAsync(page);
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

            var dataItems = items.ToList();

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;

                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                IWorksheet worksheet = workbook.Worksheets[0];

                worksheet.ImportData(dataItems, 1, 1, true);

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
    }
}

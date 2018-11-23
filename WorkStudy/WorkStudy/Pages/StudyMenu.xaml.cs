using System.IO;
using Plugin.Messaging;
using Syncfusion.XlsIO;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class StudyMenu : ContentPage
    {
        public StudyMenu()
        {
            InitializeComponent();

            ButtonEmail.Clicked += async (sender, e) =>
            {
                try
                {
                    var filePath = CreateExcelWorkBook();

                    var email1 = new EmailMessageBuilder()
                        //.To("to.plugins@xamarin.com")
                        .To(email.Text)
                        //.Cc("cc.plugins@xamarin.com")
                        //.Bcc(new[] { "bcc1.plugins@xamarin.com", "bcc2.plugins@xamarin.com" })
                        .Subject("Xamarin Messaging Plugin")
                        .Body("Well hello there from Xam.Messaging.Plugin")
                        .WithAttachment(Path.Combine(filePath, "GettingStared.xlsx"),"application/msexcel")
                        .Build();

                    var emailTask = CrossMessaging.Current.EmailMessenger;
                    if (emailTask.CanSendEmail)
                        //emailTask.SendEmail(email.Text, "Hello there!", "This was sent from the Xamrain Messaging Plugin from shared code!");
                        emailTask.SendEmail(email1);
                    else
                        await DisplayAlert("Error", "This device can't send emails", "OK");
                }
                catch
                {
                    await DisplayAlert("Error", "Unable to perform action", "OK");
                }
            };

            GenerateExcel.Clicked += (sender, e) =>
            {
                //Create an instance of ExcelEngine.
                CreateExcelWorkBook();
            };
        }

        private string CreateExcelWorkBook()
        {
            string path;

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Set the default application version as Excel 2013.
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;

                //Create a workbook with a worksheet
                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                //Access first worksheet from the workbook instance.
                IWorksheet worksheet = workbook.Worksheets[0];

                //Adding text to a cell
                worksheet.Range["A1"].Text = "Hello World";

                //Save the workbook to stream in xlsx format. 
                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);

                workbook.Close();

                //Save the stream as a file in the device and invoke it for viewing
                path =  DependencyService.Get<ISave>().SaveAndView("GettingStared.xlsx", "application/msexcel", stream).Result;
            }

            return path;
        }
    }
}

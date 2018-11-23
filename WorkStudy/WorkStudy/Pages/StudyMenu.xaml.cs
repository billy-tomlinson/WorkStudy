using Plugin.Messaging;
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
                    //var email = new EmailMessageBuilder()
                    //    .To("to.plugins@xamarin.com")
                    //    .Cc("cc.plugins@xamarin.com")
                    //    .Bcc(new[] { "bcc1.plugins@xamarin.com", "bcc2.plugins@xamarin.com" })
                    //    .Subject("Xamarin Messaging Plugin")
                    //    .Body("Well hello there from Xam.Messaging.Plugin")
                    //    .WithAttachment()
                    //    .Build();

                    var emailTask = CrossMessaging.Current.EmailMessenger;
                    if (emailTask.CanSendEmail)
                        emailTask.SendEmail(email.Text, "Hello there!", "This was sent from the Xamrain Messaging Plugin from shared code!");
                    else
                        await DisplayAlert("Error", "This device can't send emails", "OK");
                }
                catch
                {
                    await DisplayAlert("Error", "Unable to perform action", "OK");
                }
            };
        }
    }
}

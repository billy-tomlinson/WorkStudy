﻿using System;
using System.Threading.Tasks;
using WorkStudy.Services;
using Xamarin.Forms;

namespace WorkStudy.ViewModels
{
    public class ReportsPageViewModel : BaseViewModel
    {
        public Command SendEmail { get; set; }

        public ReportsPageViewModel()
        {
            ConstructorSetUp();
        }

        private void ConstructorSetUp()
        {
            SendEmail = new Command(SendEmailDetails);
            IsPageVisible = (Utilities.StudyId > 0);
        }

        private async void SendEmailDetails()
        {
            IsBusy = true;
            IsEnabled = false;
            Opacity = 0.2;

            Utilities.MoveObservationsToHistoryTable();

            Task emailTask = Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var spreadsheet = new SpreadsheetService().CreateExcelWorkBook();
                    if (spreadsheet == null)
                    {
                        IsEnabled = true;
                        IsBusy = false;
                        Opacity = 1;

                        ValidationText = "Unable to generate report. Please check STORAGE permissions.";
                        Opacity = 0.2;
                        IsInvalid = true;
                        ShowClose = true;
                    }
                    else
                    {
                        Utilities.SendEmail(spreadsheet);
                        IsBusy = false;
                        IsEnabled = true;
                        IsPageEnabled = true;
                        Opacity = 1;
                    }

                });
            });

            await emailTask;
        }
    }
}

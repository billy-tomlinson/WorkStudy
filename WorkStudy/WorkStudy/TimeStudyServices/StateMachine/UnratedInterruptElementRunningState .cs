﻿
using System.Linq;
using TimeStudy.Services;
using TimeStudy.ViewModels;
using TimeStudyApp.Model;

namespace TimeStudyApp.Services.StateMachine
{
    public class UnratedInterruptElementRunningState : BaseState
    {
        TimeStudyMainPageViewModel viewModel;
        ApplicationState stateservice = new ApplicationState();

        public UnratedInterruptElementRunningState(TimeStudyMainPageViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void AddElementWithoutLapTimeToList()
        {

            var currentRunning = viewModel.Get_Running_LapTime();
            if (currentRunning == null)
                currentRunning = viewModel.Get_Running_LapTime_By_Id();

            currentRunning.Status = RunningStatus.Paused;
            currentRunning.TotalElapsedTimeDouble = Utilities.TimeWhenLapOrForiegnButtonClicked;
            currentRunning.TotalElapsedTime = "Paused";
            currentRunning.HasBeenPaused = true;

            viewModel.LapTimeRepo.SaveItem(currentRunning);

            var currentSelected = viewModel.CollectionOfElements.FirstOrDefault(x => x.Id == Utilities.CurrentSelectedElementId);

            var currentForeignLap = Utilities.SetUpCurrentLapTime(currentSelected, viewModel.CycleCount, 
                RunningStatus.Running);

            Utilities.CurrentRunningElementId = viewModel.LapTimeRepo.SaveItem(currentForeignLap);

            viewModel.CurrentElementWithoutLapTimeName = currentForeignLap.Element;
            viewModel.CurrentSequence = null;

            viewModel.LapTimes = viewModel.Get_All_LapTimes_Not_Running();

            viewModel.Opacity = 0.2;
            viewModel.RatingsVisible = true;
            viewModel.IsForeignEnabled = false;

            viewModel.CurrentApplicationState.CurrentState = Model.Status.UnratedInterruptElementRunning;
            stateservice.SaveApplicationState(viewModel.CurrentApplicationState);
        }

        public override void ElementSelectedEvent()
        {
            viewModel.IsForeignEnabled = false;
            viewModel.CurrentApplicationState.CurrentState = Model.Status.UnratedInterruptElementRunning;
            stateservice.SaveApplicationState(viewModel.CurrentApplicationState);
        }

        public override void RatingSelectedEvent()
        {

            viewModel.IsForeignEnabled = false;
            viewModel.CollectionOfElements = viewModel.Get_All_Enabled_WorkElements_WithChildren();

            var currentSelected = viewModel.CollectionOfElements.FirstOrDefault(x => x.Id == Utilities.CurrentSelectedElementId);
            viewModel.ProcessForeignElementWithRating(currentSelected.Rated, currentSelected.Name,
               viewModel.RatingButton.Rating);

            if (viewModel.LapButtonText != "   Lap   ")
            {
                viewModel.IsPageEnabled = false;
                viewModel.ActivitiesVisible = true;
                viewModel.RatingsVisible = false;
                viewModel.Opacity = 0.2;
            }
            else
            {
                viewModel.ReInstatePausedLapTimeToCurrentRunning();
                viewModel.IsPageEnabled = true;
                viewModel.IsForeignEnabled = true;
                viewModel.ActivitiesVisible = false;
                viewModel.RatingsVisible = false;
                viewModel.Opacity = 1.0;
            }

            viewModel.CurrentApplicationState.CurrentState = Model.Status.ElementRunning;
            stateservice.SaveApplicationState(viewModel.CurrentApplicationState);
            viewModel.CollectionOfElements = viewModel.Get_All_Enabled_WorkElements_WithChildren();
        }

        public override void ShowForeignElements()
        {
            viewModel.IsCancelEnabled = true;
            viewModel.IsPageEnabled = false;

            viewModel.CollectionOfElements = viewModel.Get_All_Enabled_WorkElements_WithChildren();
            viewModel.GroupElementsForActivitiesView();

            var runningLapTime = viewModel.Get_Running_LapTime();
            if (runningLapTime.Rating == null)
            {
                viewModel.ReInstatePausedLapTimeToCurrentRunning();

                viewModel.IsForeignEnabled = false;
                viewModel.RatingsVisible = false;
                viewModel.ActivitiesVisible = true;
                viewModel.Opacity = 0.2;
   
            }
            else
            {
                viewModel.Opacity = 0.2;
                viewModel.ActivitiesVisible = true;
            }
        }

        public override void ShowStandardElements()
        {
            viewModel.IsPageEnabled = false;
            viewModel.IsCancelEnabled = false;
            viewModel.IsForeignEnabled = false;
            viewModel.RatingsVisible = false;

            viewModel.IsForeignEnabled = false;
            viewModel.CollectionOfElements = viewModel.Get_All_Enabled_WorkElements_WithChildren();

            var currentSelected = viewModel.CollectionOfElements.FirstOrDefault(x => x.Id == Utilities.CurrentSelectedElementId);
            viewModel.ProcessForeignElementWithRating(currentSelected.Rated, currentSelected.Name, 0);

            if (viewModel.LapButtonText != "   Lap   ")
            {
                viewModel.IsPageEnabled = false;
                viewModel.ActivitiesVisible = true;
                viewModel.RatingsVisible = false;
                viewModel.Opacity = 0.2;
            }
            else
            {
                viewModel.ReInstatePausedLapTimeToCurrentRunning();
                viewModel.IsPageEnabled = true;
                viewModel.IsForeignEnabled = true;
                viewModel.ActivitiesVisible = false;
                viewModel.RatingsVisible = false;
                viewModel.Opacity = 1.0;
            }

            viewModel.CurrentApplicationState.CurrentState = Model.Status.ElementRunning;
            stateservice.SaveApplicationState(viewModel.CurrentApplicationState);
            viewModel.CollectionOfElements = viewModel.Get_All_Enabled_WorkElements_WithChildren();
            viewModel.GroupElementsForActivitiesView();

        }

        public override void CloseActivitiesViewEvent()
        {
            viewModel.IsForeignEnabled = true;
        }

    }
}

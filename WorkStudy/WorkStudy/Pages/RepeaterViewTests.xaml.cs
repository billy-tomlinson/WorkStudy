using System;
using System.Collections.Generic;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy.Pages
{
    public partial class RepeaterViewTests : ContentPage
    {
        public RepeaterViewTests()
        {
            InitializeComponent();
        }

		private void Student_Tapped(object sender, EventArgs e)
        {
            var view = sender as Label;

            var student = view?.BindingContext as string;

            var vm = BindingContext as MainViewModel;

            vm.SelectStudentCommand.Execute(student);

            DisplayAlert("Tap Event", "You have selected " + view.Text, "Ok");

        }
    }
}

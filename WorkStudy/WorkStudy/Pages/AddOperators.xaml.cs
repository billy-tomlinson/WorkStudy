
using System;
using WorkStudy.Custom;
using WorkStudy.ViewModels;
using Xamarin.Forms;

namespace WorkStudy
{
    public partial class AddOperators : ContentPage
    {

        AddOperatorsViewModel viewModel;

        public AddOperators()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            viewModel = BindingContext as AddOperatorsViewModel;
            CreateActivitiesGrid();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        void CreateActivitiesGrid()
        {
            var items = viewModel.Activities;
            double count = (double)items.Count / 3;
            var rounded = Math.Ceiling(count);

            for (int i = 0; i < rounded; i++)
            {
                gridLayout.RowDefinitions.Add(new RowDefinition());
            }

            gridLayout.ColumnDefinitions.Add(new ColumnDefinition());
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition());
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition());

            var productIndex = 0;

            for (int rowIndex = 0; rowIndex < rounded; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 3; columnIndex++)
                {
                    if (productIndex >= items.Count)
                    {
                        break;
                    }

                    var product = items[productIndex];
                    productIndex += 1;

                    var label = new CustomButton
                    {
                        Text = product.Name,
                        ActivityId = product.Id,
                        Command = viewModel.ActivitySelected,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        CommandParameter = product.Id

                    };
                    gridLayout.Children.Add(label, columnIndex, rowIndex);
                }
            }
        }
    }
}

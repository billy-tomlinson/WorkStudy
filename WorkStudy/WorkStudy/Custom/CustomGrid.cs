﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkStudy.Model;
using Xamarin.Forms;

namespace WorkStudy.Custom
{
    public class CustomGrid : Grid
    {
       
         public static readonly BindableProperty ActivitiesProperty =  
           BindableProperty.Create(  
               nameof(Activities),  
               typeof(ObservableCollection<Activity>),
               typeof(CustomGrid),
               //propertyChanged:ImageSourcePropertyChanged,
               defaultValueCreator:null);  
  
        public ObservableCollection<Activity> Activities  
        {  
            get { return (ObservableCollection<Activity>)GetValue(ActivitiesProperty); }  
            set { SetValue(ActivitiesProperty, value); }  
        } 


        public static readonly BindableProperty CommandProperty =  
           BindableProperty.Create(  
               nameof(Command),  
               typeof(ICommand),
               typeof(CustomGrid),
               //propertyChanged:ImageSourcePropertyChanged,
               defaultValueCreator:null);  
  
        public ICommand Command  
        {  
            get { return (ICommand)GetValue(CommandProperty); }  
            set { SetValue(CommandProperty, value); }  
        }

        public CustomGrid()
        {
            CreateActivitiesGrid(Activities);
        }

        protected override void OnParentSet()
        {
            CreateActivitiesGrid(Activities);
            base.OnParentSet();
        }
        private static void ImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
           
            var xx = newValue as ObservableCollection<Activity>;
            var control = (CustomGrid)bindable;
            //control.CreateActivitiesGrid(xx);
        }

        public void CreateActivitiesGrid(ObservableCollection<Activity> items)
        {
            if (items == null)
                return;

            double count = (double)items.Count / 3;

            var rounded = Math.Ceiling(count);

            for (int i = 0; i < rounded; i++)
            {
                RowDefinitions.Add(new RowDefinition());
            }

            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());

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
                        Command = Command,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        CommandParameter = product.Id

                    };

                    Children.Add(label, columnIndex, rowIndex);
                }
            }

        }

    }
}


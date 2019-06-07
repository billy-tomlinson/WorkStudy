﻿using System.ComponentModel;
using Xamarin.Forms;

namespace TimeStudy.Custom
{
    [DesignTimeVisible(true)]
    public class CustomImage : Image
    {
        public static readonly BindableProperty OperatorIdProperty =  
           BindableProperty.Create(  
               nameof(OperatorId),  
               typeof(int),  
               typeof(CustomLabel),  
               0);  
  
        public int OperatorId  
        {  
            get { return (int)GetValue(OperatorIdProperty); }  
            set { SetValue(OperatorIdProperty, value); }  
        } 
    }
}

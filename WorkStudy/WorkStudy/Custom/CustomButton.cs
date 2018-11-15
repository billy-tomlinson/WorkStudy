using Xamarin.Forms;

namespace WorkStudy.Custom
{
    public class CustomButton: Button  
    {  
         public static readonly BindableProperty ActivityIdProperty =  
           BindableProperty.Create(  
               nameof(ActivityId),  
               typeof(int),  
               typeof(CustomButton),  
               0);  
  
        public int ActivityId  
        {  
            get { return (int)GetValue(ActivityIdProperty); }  
            set { SetValue(ActivityIdProperty, value); }  
        }  
    }  
}

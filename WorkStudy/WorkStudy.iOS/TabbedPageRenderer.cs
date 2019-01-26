﻿using System;
using UIKit;
using WorkStudy.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]
namespace WorkStudy.iOS
{
    public class TabbedPageRenderer : TabbedRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            TabBar.UnselectedItemTintColor = UIColor.White;
            TabBar.Translucent = false;
            TabBar.Opaque = true;

            //TabBar.SetBackgroundImage(new UIKit.UIImage(), UIKit.UIBarMetrics.Default);
            //TabBar.ShadowImage = new UIKit.UIImage();

        }
    }
}

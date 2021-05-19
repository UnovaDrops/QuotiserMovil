using Syncfusion.XForms.iOS.ComboBox;
using Syncfusion.XForms.iOS.Core;
using Syncfusion.SfRating.XForms.iOS;
using Syncfusion.XForms.iOS.Cards;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.XForms.iOS.Graphics;
using Syncfusion.XForms.iOS.Expander;
using Syncfusion.SfChart.XForms.iOS.Renderers;
using Syncfusion.XForms.iOS.Border;
using Syncfusion.XForms.iOS.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.iOS.Buttons;

using Foundation;
using UIKit;
using Syncfusion.XForms.iOS.EffectsView;

namespace QuotiserAppMobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            SfRatingRenderer.Init();
            SfComboBoxRenderer.Init();
            SfCardViewRenderer.Init();
            SfListViewRenderer.Init();
            SfGradientViewRenderer.Init();
            SfSegmentedControlRenderer.Init();
            SfExpanderRenderer.Init();
            SfChartRenderer.Init();
            SfBorderRenderer.Init();
            SfButtonRenderer.Init();
            SfListViewRenderer.Init();
            SfEffectsViewRenderer.Init();
            Syncfusion.XForms.iOS.Cards.SfCardViewRenderer.Init();
            new Syncfusion.SfNavigationDrawer.XForms.iOS.SfNavigationDrawerRenderer();

            LoadApplication(new App());
            Syncfusion.XForms.iOS.Buttons.SfCheckBoxRenderer.Init();

            return base.FinishedLaunching(app, options);
        }
    }
}

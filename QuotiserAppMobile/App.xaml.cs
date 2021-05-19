using QuotiserAppMobile.Views.Login;
using QuotiserAppMobile.Views.Panel;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace QuotiserAppMobile
{
    public partial class App : Application
    {
        [Obsolete]
        public static MasterDetailPage MasterD { get; set; }

        public App()
        {                     
            InitializeComponent();
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDIyOTA1QDMxMzkyZTMxMmUzMGpDY0NSbGFMcjJ6NnRHMDd0RS9ER09STVVKQ29keFAxQVBydWNDenFCNVU9");
            if (Application.Current.Properties.ContainsKey("IsLogin"))
            {
                if (App.Current.Properties["IsLogin"].Equals("true"))
                {
                   
                    MainPage = new PanelVista();
                }
                else
                {
                  
                    MainPage = new LoginVista();
                }
            }
            else
            {
               
                MainPage = new LoginVista();
            }
         
        }    
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

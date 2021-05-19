using Android.OS;
using Android.App;
using QuotiserAppMobile.Droid;
using Android.Content.PM;


namespace SplashScreenDemo.Droid
{
    [Activity(Theme = "@style/SplashTheme", Label = "Quotiser", Icon = "@mipmap/quotisericono", MainLauncher = true, NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));

            
        }

      
    }
}
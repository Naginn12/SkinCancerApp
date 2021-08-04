using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using SkinCancerApp.Views.login.LoginPage;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SkinCancerApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=26360421-4b7c-4160-bfc2-de8fa14049f9;" +
          
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

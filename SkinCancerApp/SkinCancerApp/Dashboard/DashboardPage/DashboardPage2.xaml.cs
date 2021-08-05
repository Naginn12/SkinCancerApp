using System;
using System.Collections.Generic;
using SkinCancerApp.ViewModel.DashboardPage;
using Xamarin.Forms;

namespace SkinCancerApp.Dashboard.DashboardPage
{
    public partial class DashboardPage2 : ContentPage
    {
        public DashboardPage2()
        {
            InitializeComponent();

            BindingContext = new DashboardPage2ViewModel();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SkinCancerApp.Dashboard.DashboardPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyDashboardTabbedPage : TabbedPage
    {
        public MyDashboardTabbedPage()
        {
            InitializeComponent();
        }
    }
}

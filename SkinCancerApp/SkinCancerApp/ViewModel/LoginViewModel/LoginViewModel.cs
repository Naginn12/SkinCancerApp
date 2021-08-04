using SkinCancerApp.Dashboard.DashboardPage;
using SkinCancerApp.Services.LoginApiServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SkinCancerApp.ViewModel.LoginViewModel
{
  public  class LoginViewModel : BindableObject
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public INavigation Navigation { get; }

        public LoginViewModel(INavigation navigation)
        {
            Navigation = navigation;
            LoginCommand = new Command(async () => await Login());
        }

        private async Task Login()
        {
            var response = await LoginApiServices.ServiceClientInstance.LoginUser(Name, Password);

            if (response != null)
            {

                await Navigation.PushAsync(new DashboardPage());

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Incorrect Password!!!", "Ok");
            }
        }
    }
}

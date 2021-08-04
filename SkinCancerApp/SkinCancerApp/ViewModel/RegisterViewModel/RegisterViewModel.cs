using SkinCancerApp.Dashboard.DashboardPage;
using SkinCancerApp.Services.LoginApiServices;
using SkinCancerApp.Services.RegisterApiServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SkinCancerApp.ViewModel.RegisterViewModel
{
  public  class RegisterViewModel : BindableObject
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
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }
        private string contactno;
        public string Contactno
        {
            get { return contactno; }
            set
            {
                contactno = value;
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

        public ICommand RegisterCommand { get; }


        public RegisterViewModel()
        {
            RegisterCommand = new Command(async () => await SetRegisterUser());
        }

        private async Task SetRegisterUser()
        {
            var response = await RegisterApiServices.ServiceClientInstance.RegisterUser(Name, Email, Contactno, Password);

            if (response == true)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "User Created Sucessfully", "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Alert", "User Created Sucessfully", "Ok");
            }
        }
    }
}

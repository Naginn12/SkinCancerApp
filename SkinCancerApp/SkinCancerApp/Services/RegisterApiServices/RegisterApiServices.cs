using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using SkinCancerApp.Model.RegisterModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkinCancerApp.Services.RegisterApiServices
{
   public class RegisterApiServices
    {
        private JsonSerializer _serializer = new JsonSerializer();

        private static RegisterApiServices _ServiceClientInstance;

        public static RegisterApiServices ServiceClientInstance
        {
            get
            {
                if (_ServiceClientInstance == null)
                    _ServiceClientInstance = new RegisterApiServices();
                return _ServiceClientInstance;
            }
        }

        FirebaseClient firebase;
        public RegisterApiServices()
        {
            //replace this with your firebase realtimedatabase end point.
            firebase = new FirebaseClient("https://skincancerdiagnosis-797ca-default-rtdb.firebaseio.com/");
        }

        public async Task<bool> RegisterUser(string name, string email, string contactno, string password)
        {
            var result = await firebase
                .Child("RegisterUsersTable")
                .PostAsync(new RequestModel() { Name = name, Email = email, Contactno = contactno, Password = password });

            if (result.Object != null)
            {
                return true;
            }
            else
            {
                return false;

            }
        }
    }
}

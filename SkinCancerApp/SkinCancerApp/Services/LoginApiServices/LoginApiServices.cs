using Firebase.Database;
using Newtonsoft.Json;
using SkinCancerApp.Model.Loginmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCancerApp.Services.LoginApiServices
{
   public class LoginApiServices
    {
        private JsonSerializer _serializer = new JsonSerializer();


        private static LoginApiServices _ServiceClientInstance;

        public static LoginApiServices ServiceClientInstance
        {
            get
            {
                if (_ServiceClientInstance == null)
                    _ServiceClientInstance = new LoginApiServices();
                return _ServiceClientInstance;
            }
        }

        FirebaseClient firebase = new FirebaseClient("https://skincancerdiagnosis-797ca-default-rtdb.firebaseio.com/");



        //public async Task<List<ResponseModel>> GetAllDATA()
        //{
        //    return (await firebase
        //        .Child("Teacher")
        //        .OnceAsync<ResponseModel>()).Select(item => new ResponseModel
        //        {
        //            Email = item.Object.Email,
        //            Password = item.Object.Password
        //        }).ToList();
        //}

        public async Task<ResponseModel> LoginUser(string email, string password)
        {
            //var alldata = await GetAllDATA();

            //await firebase
            //  .Child("RegisterUsersTable")
            //  .OnceAsync<ResponseModel>(); 
            //return alldata.Where(a => a.Email == email).Where(b => b.Password == password).FirstOrDefault();

            var GetPerson = (await firebase
             .Child("RegisterUsersTable")
             .OnceAsync<ResponseModel>()).Where(a => a.Object.Email == email).Where(b => b.Object.Password == password).FirstOrDefault();

            if (GetPerson != null)
            {

                var content = GetPerson.Object as ResponseModel;
                return content;

            }
            else
            {
                return null;
            }

        }
    }
}

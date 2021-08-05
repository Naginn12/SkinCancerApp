using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SkinCancerApp.ViewModel.DashboardPage
{
    public class DashboardPage2ViewModel : BindableObject
    {
        private string myphoto;
        public string Myphoto
        {
            get { return myphoto; }
            set
            {
                myphoto = value;
                OnPropertyChanged();
            }
        }

        private string result1;
        public string Result1
        {
            get { return result1; }
            set
            {
                result1 = value;
                OnPropertyChanged();
            }
        }
        private string result2;
        public string Result2
        {
            get { return result2; }
            set
            {
                result2 = value;
                OnPropertyChanged();
            }
        }
        private string result3;
        public string Result3
        {
            get { return result3; }
            set
            {
                result3 = value;
                OnPropertyChanged();
            }
        }
        private string result4;
        public string Result4
        {
            get { return result4; }
            set
            {
                result4 = value;
                OnPropertyChanged();
            }
        }
        private string result5;
        public string Result5
        {
            get { return result5; }
            set
            {
                result5 = value;
                OnPropertyChanged();
            }
        }
        private string result6;
        public string Result6
        {
            get { return result6; }
            set
            {
                result6 = value;
                OnPropertyChanged();
            }
        }
        //--- Computer Vision API's
        static string subscriptionKey = "ddbdae8a57624a92b5606d3a80fac0da";
        static string endpoint = "https://imageclassifer.cognitiveservices.azure.com/";

        //-- End of Computer Vision Api's Keys

        public ICommand TakePhotoCommand { get; }
        public ICommand UploadPhotoCommand { get; }



        public DashboardPage2ViewModel()
        {

            TakePhotoCommand = new Command(async () => await TakeImage());
            UploadPhotoCommand = new Command(async () => await UploadImage());

        }

        private async Task UploadImage()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                await LoadPhotoAsync(photo);
                //Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is now supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }
        string PhotoPath;

        public async Task LoadPhotoAsync(FileResult photo)
        {

            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }

            PhotoPath = photo.FullPath;
            Myphoto = PhotoPath;

            MakePredictionRequest(PhotoPath);

        }

        public async Task TakeImage()
        {
            var result = await MediaPicker.CapturePhotoAsync();
            await TakePhotoAsync(result);

        }
        public async Task TakePhotoAsync(FileResult result)
        {
            if (result == null)
            {
                PhotoPath = null;
                return;
            }
            PhotoPath = result.FullPath;
            Myphoto = PhotoPath;

            MakePredictionRequest(PhotoPath);
        }

        public async Task MakePredictionRequest(string imageFilePath)
        {
            try
            {
                ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscriptionKey))
              {
                  Endpoint = endpoint
              };

                List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
                       {
                           VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                           VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                           VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                           VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                           VisualFeatureTypes.Objects
                       };
                ImageAnalysis results = await client.AnalyzeImageInStreamAsync(File.OpenRead(imageFilePath), visualFeatures: features);
                Result1 = results?.Tags[0]?.Name.ToString();
                Result2 = results?.Tags[1]?.Name.ToString();
                Result3 = results?.Tags[2]?.Name.ToString();
                Result4 = results?.Tags[3]?.Name.ToString();
                Result5 = results?.Tags[4]?.Name.ToString();
                Result6 = results?.Tags[5]?.Name.ToString();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");

            }
        }
    }
}

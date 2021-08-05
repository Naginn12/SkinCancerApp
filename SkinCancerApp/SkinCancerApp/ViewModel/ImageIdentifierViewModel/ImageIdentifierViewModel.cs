using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SkinCancerApp.Model.ImageModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SkinCancerApp.ViewModel.ImageIdentifierViewModel
{
  public  class ImageIdentifierViewModel : BindableObject
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
        private double nonSkinCancerPercentage;
        public double NonSkinCancerPercentage
        {
            get { return nonSkinCancerPercentage; }
            set
            {
                nonSkinCancerPercentage = value;
                OnPropertyChanged();
            }
        }
        private double skinCancerPercentage;
        public double SkinCancerPercentage
        {
            get { return skinCancerPercentage; }
            set
            {
                skinCancerPercentage = value;
                OnPropertyChanged();
            }
        }

        private bool predectionBoardVisibility =false;
        public bool PredectionBoardVisibility
        {
            get { return predectionBoardVisibility; }
            set
            {
                predectionBoardVisibility = value;
                OnPropertyChanged();
            }
        }
        private bool isDangeredSkinFrameVisible = false;
        public bool IsDangeredSkinFrameVisible
        {
            get { return isDangeredSkinFrameVisible; }
            set
            {
                isDangeredSkinFrameVisible = value;
                OnPropertyChanged();
            }
        }
        private bool isNormalSkinFrameVisible = false;
        public bool IsNormalSkinFrameVisible
        {
            get { return isNormalSkinFrameVisible; }
            set
            {
                isNormalSkinFrameVisible = value;
                OnPropertyChanged();
            }
        }

        


        //--- Computer Vision API's
        //static string subscriptionKey = "ddbdae8a57624a92b5606d3a80fac0da";
        //static string endpoint = "https://imageclassifer.cognitiveservices.azure.com/";

        //-- End of Computer Vision Api's Keys


        private static string predictionEndpoint = "https://southcentralus.api.cognitive.microsoft.com/customvision/v3.0/Prediction/e03446b6-78d0-4be2-90fd-865ae4644687/classify/iterations/Iteration1/image";
        private static string predictionKey = "812509a6ea1a4bc1a43fdb6cce26a9c4";
        // You can obtain this value from the Properties page for your Custom Vision Prediction resource in the Azure Portal. See the "Resource ID" field. This typically has a value such as:
        // /subscriptions/<your subscription ID>/resourceGroups/<your resource group>/providers/Microsoft.CognitiveServices/accounts/<your Custom Vision prediction resource name>
        private static string predictionResourceId = "/subscriptions/93797f48-b75d-400f-a223-47f5f561c91e/resourceGroups/ImageClassificatioResourcesGroup/providers/Microsoft.CognitiveServices/accounts/Nagin";

        public ICommand TakePhotoCommand { get; }
        public ICommand UploadPhotoCommand { get; }





        public ImageIdentifierViewModel()
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
            // save the file into local storage this is only for Capturing Image and saving to the local directory
            //var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            //using (var stream = await photo.OpenReadAsync())
            //using (var newStream = File.OpenWrite(newFile))
            //    await stream.CopyToAsync(newStream);

            Myphoto = " ";
            await Task.Delay(1000);
            PhotoPath = photo.FullPath;
            Myphoto = PhotoPath;





            //   //-- Start of Computer Vision   --------------------
            //   ComputerVisionClient client =
            //new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscriptionKey))
            //{
            //    Endpoint = endpoint };

            //   List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            //       {
            //           VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            //           VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            //           VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
            //           VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
            //           VisualFeatureTypes.Objects
            //       };
            //   ImageAnalysis results = await client.AnalyzeImageAsync("https://res.cloudinary.com/demo/image/upload/w_0.5,ar_1,c_thumb,g_faces,z_0.7/r_max/young_couple2.jpg", visualFeatures: features);
            //   Result1.Text = results?.Faces[0]?.Gender.ToString();
            //   Result2.Text = results?.Faces[1]?.Gender.ToString();

            // //  ------------------------------------------------------- End of Computer Vision



            //-- Stating of Custom Vision ------

            MakePredictionRequest(PhotoPath);

        }



        public async Task TakeImage()
        {
            //var result = await MediaPicker.CapturePhotoAsync();
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg",
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small
            });
            await TakePhotoAsync(file);

        }

        public async Task TakePhotoAsync(MediaFile result)
        {
            //if (result == null)
            //{
            //    PhotoPath = null;
            //    return;
            //}
            Myphoto = " ";
            await Task.Delay(1000);

            PhotoPath = result.Path;
            Myphoto = PhotoPath;

            MakePredictionRequest(PhotoPath);
        }
     



        public  async Task MakePredictionRequest(string imageFilePath)
        {
            try
            {
                var client = new HttpClient();

                // Request headers - replace this example key with your valid Prediction-Key.
                client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

                // Prediction URL - replace this example URL with your valid Prediction URL.
                string url = predictionEndpoint;

                HttpResponseMessage response;

                // Request body. Try this sample with a locally stored image.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(url, content);

                    var outputcontent = await response.Content.ReadAsStringAsync();

                    var reponsecontent = JsonConvert.DeserializeObject<ImageResponseModel>(outputcontent);

                    //Rslt1.Text = reponsecontent.Predictions[0].TagName.ToString() + reponsecontent.Predictions[0].Probability.ToString();
                    //Rslt2.Text = reponsecontent.Predictions[1].TagName.ToString() + reponsecontent.Predictions[1].Probability.ToString();
                    var skincancerval = reponsecontent.Predictions[0].Probability;
                    var nonskincancerval = reponsecontent.Predictions[1].Probability;
                    SkinCancerPercentage = skincancerval;
                    NonSkinCancerPercentage = nonskincancerval;

                    PredectionBoardVisibility = true;

                    //Your Logic Lies here :

                    // if double <= 30 then let the user know that user is safe contact doctor for further medical assistance

                    if (SkinCancerPercentage <= 30)
                    {
                        PredectionBoardVisibility = true;
                        IsDangeredSkinFrameVisible = false;
                        IsNormalSkinFrameVisible = true;


                    }
                    else
                    {
                        //or show the frame which has told the user that skin cancer detected.. 
                        PredectionBoardVisibility = true;
                        IsDangeredSkinFrameVisible = false;
                        IsNormalSkinFrameVisible = true;

                    }
                }
            }
            catch (Exception ex)
            {

                PredectionBoardVisibility = false;
                IsDangeredSkinFrameVisible = false;
                IsNormalSkinFrameVisible = false;
                await App.Current.MainPage.DisplayAlert("Alert",ex.Message,"Ok");
            }
        }

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}

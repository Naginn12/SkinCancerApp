using Newtonsoft.Json;
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

        private async Task LoadPhotoAsync(FileResult photo)
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



        private async Task TakeImage()
        {
            var result = await MediaPicker.CapturePhotoAsync();
            await TakePhotoAsync(result);

        }

        private async Task TakePhotoAsync(FileResult result)
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

        public static async Task MakePredictionRequest(string imageFilePath)
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
